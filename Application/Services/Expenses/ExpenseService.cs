using IOU1.Application.Features.Expenses.AddExpense.Models;
using IOU1.Application.Services.Balances;
using IOU1.Application.Services.Members;
using IOU1.Application.Services.Settlements;
using IOU1.Application.Strategy;
using IOU1.Domain.Constants.Expense;
using IOU1.Domain.Entities;
using IOU1.Domain.Enums;
using IOU1.Domain.Models;
using IOU1.Domain.Models.Auth.User;
using IOU1.Domain.Models.Results;
using IOU1.Domain.Services.Splits;
using IOU1.Persistance.Context;
using Microsoft.EntityFrameworkCore;

namespace IOU1.Application.Services.Expenses;

public class ExpenseService(
    IOU1Context context,
    IAuthUser user,
    IBalanceService balanceService,
    IMemberService memberService,
    ISettlementService settlementService) : IExpenseService
{
    private readonly IOU1Context _context = context;
    private readonly IAuthUser _user = user;
    private readonly IBalanceService _balanceService = balanceService;
    private readonly IMemberService _memberService = memberService;
    private readonly ISettlementService _settlementService = settlementService;

    public async Task<Result<Expense?>> AddExpense(
        NewExpense newExpense,
        CancellationToken cancellationToken = default)
    {
        try
        {
            //Sprawdzmy tylko czu uzytkownik nalezy do grupy - moze z cache?
            var isMemberOfGroup = await _memberService.IsMemberOfGroup(_user.Id, newExpense.GroupId, cancellationToken);
            if (!isMemberOfGroup)
                return Result<Expense?>.Failure($"User {_user.Login} is not a member of group {newExpense.GroupId}.");

            var memberIds = new HashSet<int>();
            foreach (var split in newExpense.Splits)
            {
                if (!memberIds.Add(split.MemberId))
                    return Result<Expense?>.Failure($"Duplicate member id {split.MemberId} found in splits.");

                memberIds.Add(split.MemberId);
            }

            if (!memberIds.Contains(newExpense.PayerId))
                memberIds.Add(newExpense.PayerId);

            var group = await _context.Groups
                .Include(g => g.Members.Where(m => memberIds.Contains(m.Id)))
                    .ThenInclude(m => m.User)
                .FirstOrDefaultAsync(g =>
                    g.Id == newExpense.GroupId,
                    cancellationToken);

            if (group is null)
                return Result<Expense?>.Failure($"Group {newExpense.GroupId} couldn't be found.");

            var payingMember = group.Members.FirstOrDefault(m => m.Id == newExpense.PayerId);
            if (payingMember is null)
                return Result<Expense?>.Failure($"Buyer {newExpense.PayerId} not found in group {newExpense.GroupId}.");

            var expense = Expense.Create(
                newExpense.Title,
                newExpense.Description,
                newExpense.Amount,
                group,
                payer: payingMember,
                newExpense.Splits,
                splitStrategy: ChooseStrategy(newExpense.SplitTypeId),
                newExpense.CategoryId,
                newExpense.SplitTypeId);

            //Gdy dodajemy Expense to patrzymy na balans 
            //jesli jest ujemny lub dodatni (pomysl) to wtedy zalezy kto placi a kto nie (mozna dodac od razu settlement)
            //i ten settlement poszuka sobie expense share które zsettluje
            //czyli musi byc tabelka pomiędzy

            var expenseShareSettlements = await _settlementService.SettleBasedOnNewExpense(SearchExpense.ByOldest, expense, cancellationToken);

            //tutaj trzeba zachowac transakcyjnosc - moze na context metoda jak nie w transkacji to rzuc wyjątek - i tyle
            //Juz nie potrzeba transakcji bo wszystkie wyliczenia już w in-memory a z bazy pociagne tylko balanse i
            //zostana uwzgledniony ExpenseShares oraz Settlements
            var balances = await _balanceService.GetBalancesFor(group, cancellationToken);
            _balanceService.AdjustBalancesForNewExpense(expense, balances, expenseShareSettlements);

            _context.Expenses.Add(expense);
            await _context.SaveChangesAsync(cancellationToken);

            return Result<Expense?>.Success(expense);
        }
        catch (Exception ex)
        {
            return Result<Expense?>.Failure(ex, "An error occured while adding an expense!");
        }
    }

    public async Task<Result<GroupExpenseSummary?>> GetSummary(int groupId, CancellationToken cancellationToken = default)
    {
        var group = await _context
            .Groups
            .Include(g => g.Members)
            .SingleOrDefaultAsync(g => g.Id == groupId, cancellationToken);

        if (group is null)
            return Result<GroupExpenseSummary?>.Failure($"Group with id: {groupId} doesn't exist.");

        var summary = await _context
            .ExpenseShares
            .Include(es => es.Expense)
                .ThenInclude(e => e.Payer)
                    .ThenInclude(p => p.User)
            .Include(es => es.Member)
            .Where(es => es.Expense.GroupId == groupId && es.Member.Id != es.Expense.PayerId)
            .GroupBy(es => new
            {
                es.MemberId,
                BorrowerName = es.Member.User.FirstName,
                es.Expense.PayerId,
                BuyerName = es.Expense.Payer.User.FirstName
            })
            .Select(gr => new Debt(
                gr.Key.MemberId,
                gr.Key.BorrowerName,
                gr.Key.PayerId,
                gr.Key.BuyerName,
                gr.Sum(es => es.Amount)))
            .ToListAsync(cancellationToken);

        return Result<GroupExpenseSummary?>.Success(new GroupExpenseSummary(
            groupId,
            summary));
    }

    #region Private Methods
    private static ISplitStrategy ChooseStrategy(int splittypeId)
    {
        return splittypeId switch
        {
            SplitTypeId.Equal => new EqualSplitStrategy(),
            SplitTypeId.Custom => new CustomSplitStrategy(),
            SplitTypeId.Percentage => new PercentageSplitStrategy(),
            _ => throw new InvalidOperationException($"Invalid split type id: {splittypeId}.")
        };
    }
    #endregion
}

using IOU1.Application.Features.Expenses.AddExpense.Models;
using IOU1.Application.Strategy;
using IOU1.Domain.Entities;
using IOU1.Domain.Enums;
using IOU1.Domain.Models;
using IOU1.Domain.Models.Results;
using IOU1.Domain.Services.Splits;
using IOU1.Persistance.Context;
using Microsoft.EntityFrameworkCore;

namespace IOU1.Application.Services.Expenses;

public class ExpenseService(IOU1Context context) : IExpenseService
{
    private readonly IOU1Context _context = context;

    public async Task<Result<Expense?>> AddExpense(
        NewExpense newExpense,
        CancellationToken cancellationToken = default)
    {
        var buyer = await _context.Users.FindAsync([newExpense.BuyerId], cancellationToken);
        if (buyer is null)
        {
            return Result<Expense?>.Failure($"Buyer {newExpense.BuyerId} not found.");
        }

        var group = await _context.Groups
            .Include(g => g.Members)
                .ThenInclude(m => m.User)
            .Include(g => g.Members)
            .FirstOrDefaultAsync(g => g.Id == newExpense.GroupId, cancellationToken);

        if (group is null)
        {
            return Result<Expense?>.Failure($"Group {newExpense.GroupId} not found.");
        }

        var expense = new Expense(
            totalAmount: newExpense.Amount,
            newExpense.Title,
            newExpense.Description,
            group,
            buyer,
            currency: await _context.Currencies.SingleAsync(c => c.Key == "PLN", cancellationToken: cancellationToken),
            newExpense.Splits.Where(s => s.MemberId != 0),
            splitStrategy: ChooseStrategy(newExpense.SplitType));

        _context.Expenses.Add(expense);
        await _context.SaveChangesAsync(cancellationToken);

        return Result<Expense?>.Success(expense);
    }

    public async Task<Result<GroupExpenseSummary?>> GetSummary(long groupId, CancellationToken cancellationToken = default)
    {
        var group = await _context
            .Groups
            .Include(g => g.Members)
            .SingleOrDefaultAsync(g => g.Id == groupId, cancellationToken);

        if (group is null)
            return Result<GroupExpenseSummary?>.Failure($"Group with id: {groupId} doesn't exist.");

        var summary = await _context
            .Transactions
            .Include(t => t.Expense)
            .Include(t => t.Borrower)
            .Include(t => t.Buyer)
            .Where(t => t.Expense.GroupId == groupId)
            .GroupBy(t => new
            {
                t.BorrowerMemberId,
                BorrowerName = t.Borrower.FirstName,
                t.BuyerMemberId,
                BuyerName = t.Buyer.FirstName
            })
            .Select(gr => new Debt(
                gr.Key.BorrowerMemberId,
                gr.Key.BorrowerName,
                gr.Key.BuyerMemberId,
                gr.Key.BuyerName,
                gr.Sum(t => t.Amount)))
            .ToListAsync(cancellationToken);

        return Result<GroupExpenseSummary?>.Success(new GroupExpenseSummary(
            groupId,
            summary));
    }

    private static ISplitStrategy ChooseStrategy(ExpenseSplitType splitType)
    {
        return splitType switch
        {
            ExpenseSplitType.Equal => new EqualSplitStrategy(),
            ExpenseSplitType.Custom => new CustomSplitStrategy(),
            _ => new CustomSplitStrategy()
        };
    }
}

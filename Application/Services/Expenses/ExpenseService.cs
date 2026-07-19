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
        var group = await _context.Groups
            .Include(g => g.Members)
                .ThenInclude(m => m.User)
            .FirstOrDefaultAsync(g => g.Id == newExpense.GroupId, cancellationToken);

        if (group is null)
        {
            return Result<Expense?>.Failure($"Group {newExpense.GroupId} not found.");
        }

        var payer = group.Members.FirstOrDefault(m => m.UserId == newExpense.BuyerId);
        if (payer is null)
        {
            return Result<Expense?>.Failure($"Buyer {newExpense.BuyerId} not found in group {newExpense.GroupId}.");
        }

        var expense = new Expense(
            totalAmount: newExpense.Amount,
            newExpense.Title,
            newExpense.Description,
            group,
            payer,
            newExpense.Splits.Where(s => s.MemberId != 0),
            splitStrategy: ChooseStrategy(newExpense.SplitType));

        _context.Expenses.Add(expense);
        await _context.SaveChangesAsync(cancellationToken);

        return Result<Expense?>.Success(expense);
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

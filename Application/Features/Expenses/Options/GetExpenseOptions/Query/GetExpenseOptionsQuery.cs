using IOU1.Application.Features.Expenses.Options.GetExpenseOptions.Models;
using IOU1.Persistance.Context;
using Microsoft.EntityFrameworkCore;

namespace IOU1.Application.Features.Expenses.Options.GetExpenseOptions.Query;

public sealed class GetExpenseOptionsQuery(
    IOU1Context context) : IGetExpenseOptionsQuery
{
    private readonly IOU1Context _context = context;

    public async Task<IEnumerable<ExpenseOption>> Get(int groupId, CancellationToken cancellationToken = default)
    {
        var expenseCategories = await _context
            .ExpenseCategories
            .Where(ec => ec.GroupId == null || ec.GroupId == groupId)
            .Select(ec => new ExpenseOption(ec.Id, ExpenseOption.ExpenseCategory, ec.Title, ec.IconKey))
            .ToListAsync(cancellationToken);

        var expenseSplits = await _context
            .ExpenseSplits
            .Select(es => new ExpenseOption(es.Id, ExpenseOption.ExpenseSplit, es.Title, es.IconKey))
            .ToListAsync(cancellationToken);

        return expenseCategories.Concat(expenseSplits);
    }
}

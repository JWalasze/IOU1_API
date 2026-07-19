using IOU1.Application.Features.Expenses.Options.GetExpenseOptions.Models;

namespace IOU1.Application.Features.Expenses.Options.GetExpenseOptions.Query;

public interface IGetExpenseOptionsQuery
{
    Task<IEnumerable<ExpenseOption>> Get(
        int groupId,
        CancellationToken cancellationToken = default);
}

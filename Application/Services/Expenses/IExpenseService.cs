using IOU1.Application.Features.Expenses.AddExpense.Models;
using IOU1.Domain.Entities;
using IOU1.Domain.Models;
using IOU1.Domain.Models.Results;

namespace IOU1.Application.Services.Expenses;

public interface IExpenseService
{
    Task<Result<Expense?>> AddExpense(
        NewExpense newExpense,
        CancellationToken cancellationToken = default);

    Task<Result<GroupExpenseSummary?>> GetSummary(int groupId, CancellationToken cancellationToken = default);
}

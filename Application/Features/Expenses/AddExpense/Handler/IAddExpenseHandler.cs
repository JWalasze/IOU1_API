using IOU1.Application.Features.Expenses.AddExpense.Models;
using IOU1.Domain.Models.Results;

namespace IOU1.Application.Features.Expenses.AddExpense.Handler;

public interface IAddExpenseHandler
{
    Task<Result<AddExpenseResponse?>> Handle(AddExpenseRequest request, CancellationToken cancellationToken = default);
}

using IOU1.Application.Features.Expenses.AddExpense.Models.Request;
using IOU1.Application.Features.Expenses.AddExpense.Models.Response;
using IOU1.Domain.Models.Results;

namespace IOU1.Application.Features.Expenses.AddExpense.Handler;

public interface IAddExpenseHandler
{
    Task<Result<AddExpenseResponse?>> Handle(AddExpenseRequest request, CancellationToken cancellationToken = default);
}

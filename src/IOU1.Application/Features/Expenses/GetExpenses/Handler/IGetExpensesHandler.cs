using IOU1.Application.Features.Expenses.GetExpenses.Models;
using IOU1.Domain.Models.Results;

namespace IOU1.Application.Features.Expenses.GetExpenses.Handler;

public interface IGetExpensesHandler
{
    Task<Result<GetExpensesResponse?>> Handle(GetExpensesRequest request, CancellationToken cancellationToken = default);
}

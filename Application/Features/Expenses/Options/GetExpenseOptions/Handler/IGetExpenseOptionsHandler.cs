using IOU1.Application.Features.Expenses.Options.GetExpenseOptions.Models.Request;
using IOU1.Application.Features.Expenses.Options.GetExpenseOptions.Models.Response;
using IOU1.Domain.Models.Results;

namespace IOU1.Application.Features.Expenses.Options.GetExpenseOptiions.Handler;

public interface IGetExpenseOptionsHandler
{
    Task<Result<IEnumerable<GetExpenseOptionsResponse>>> Handle(
        GetExpenseOptionsRequest request,
        CancellationToken cancellationToken = default);
}

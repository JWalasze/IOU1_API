using FluentValidation;
using IOU1.Application.Features.Expenses.Options.GetExpenseOptions.Models.Request;
using IOU1.Application.Features.Expenses.Options.GetExpenseOptions.Models.Response;
using IOU1.Application.Features.Expenses.Options.GetExpenseOptions.Query;
using IOU1.Domain.Models.Auth.User;
using IOU1.Domain.Models.Results;

namespace IOU1.Application.Features.Expenses.Options.GetExpenseOptiions.Handler;

public sealed class GetExpenseOptionsHandler(
    IAuthUser user,
    IValidator<GetExpenseOptionsRequest> validator,
    IGetExpenseOptionsQuery queryHandler) : IGetExpenseOptionsHandler
{
    private readonly IAuthUser _user = user;
    private readonly IValidator<GetExpenseOptionsRequest> _validator = validator;
    private readonly IGetExpenseOptionsQuery _queryHandler = queryHandler;

    public async Task<Result<IEnumerable<GetExpenseOptionsResponse>>> Handle(
        GetExpenseOptionsRequest request,
        CancellationToken cancellationToken = default)
    {
        var validationResult = _validator.Validate(request);
        if (!validationResult.IsValid)
            return Result<IEnumerable<GetExpenseOptionsResponse>>.Failure(validationResult.Errors);

        var result = await _queryHandler.Get(request.GroupId, cancellationToken);
        return Result<IEnumerable<GetExpenseOptionsResponse>>.Success(
            result.Select(r => new GetExpenseOptionsResponse(r.Id, r.Key, r.Value, r.IconKey)));
    }
}

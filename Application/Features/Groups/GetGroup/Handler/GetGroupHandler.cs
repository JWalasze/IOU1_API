using FluentValidation;
using IOU1.Application.Features.Groups.GetGroup.Models.Dto;
using IOU1.Application.Features.Groups.GetGroup.Models.Request;
using IOU1.Application.Features.Groups.GetGroup.Query;
using IOU1.Domain.Models.Results;

namespace IOU1.Application.Features.Groups.GetGroup.Handler;

public sealed class GetGroupHandler(
    IGetGroupQuery repository,
    IValidator<GetGroupRequest> validator)
    : IGetGroupHandler
{
    private readonly IGetGroupQuery _repository = repository;
    private readonly IValidator<GetGroupRequest> _validator = validator;

    public async Task<Result<GetGroupDto?>> Handle(GetGroupRequest request, CancellationToken cancellationToken = default)
    {
        var validationResult = _validator.Validate(request);
        if (!validationResult.IsValid)
        {
            return Result<GetGroupDto?>.Failure(
                validationResult.Errors.Select(e => new ProblemDetails(e.ErrorMessage, e.ErrorCode)));
        }

        var group = await _repository.GetGroup(request.GroupId, cancellationToken);
        if (group is null)
        {
            return Result<GetGroupDto?>.Failure($"Group with id: {request.GroupId} not found.");
        }

        var expenses = await _repository.GetLastExpenses(request.GroupId, lastExpenseCount: 10, cancellationToken);
        var groupWithExpenses = group with { Expenses = [.. expenses] };

        return Result<GetGroupDto?>.Success(groupWithExpenses);
    }
}

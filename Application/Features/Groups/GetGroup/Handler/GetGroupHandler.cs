using FluentValidation;
using IOU1.Application.Features.Groups.GetGroup.Models.Dto;
using IOU1.Application.Features.Groups.GetGroup.Models.Request;
using IOU1.Application.Features.Groups.GetGroup.Query;
using IOU1.Domain.Models.Auth.User;
using IOU1.Domain.Models.Results;

namespace IOU1.Application.Features.Groups.GetGroup.Handler;

public sealed class GetGroupHandler(
    IAuthUser user,
    IGetGroupQuery repository,
    IValidator<GetGroupRequest> validator)
    : IGetGroupHandler
{
    private readonly IAuthUser _user = user;
    private readonly IGetGroupQuery _repository = repository;
    private readonly IValidator<GetGroupRequest> _validator = validator;

    public async Task<Result<GroupDto?>> Handle(GetGroupRequest request, CancellationToken cancellationToken = default)
    {
        var validationResult = _validator.Validate(request);
        if (!validationResult.IsValid)
            return Result<GroupDto?>.Failure(validationResult.Errors);

        var isItYourGroup = await _repository.IsThatYourGroup(_user.Id, request.GroupId, cancellationToken);
        if (!isItYourGroup)
            return Result<GroupDto?>.Failure($"You are not a member of the group with id: {request.GroupId}.");

        var group = await _repository.GetGroup(request.GroupId, cancellationToken);
        if (group is null)
            return Result<GroupDto?>.Failure($"Group with id: {request.GroupId} not found.");

        var expenses = await _repository.GetLastExpenses(request.GroupId, lastExpenseCount: 10, cancellationToken);
        var groupWithExpenses = group with { Expenses = [.. expenses] };

        var balances = await _repository.GetBalances(request.GroupId, cancellationToken);
        var groupWithBalances = groupWithExpenses with { Balances = [.. balances.Where(b => b.Amount != 0)] };

        return Result<GroupDto?>.Success(groupWithBalances);
    }
}

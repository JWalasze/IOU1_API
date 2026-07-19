using FluentValidation;
using IOU1.Application.Features.Groups.AddGroup.Models.Endpoint;
using IOU1.Application.Services.Groups;
using IOU1.Domain.Models.Results;

namespace IOU1.Application.Features.Groups.AddGroup.Handler;

public sealed class AddGroupHandler(
    IValidator<AddGroupRequest> validator,
    IGroupService groupService)
    : IAddGroupHandler
{
    private readonly IValidator<AddGroupRequest> _validator = validator;
    private readonly IGroupService _groupService = groupService;

    public async Task<Result<AddGroupResponse?>> Handle(AddGroupRequest request, CancellationToken cancellationToken = default)
    {
        var validationResult = _validator.Validate(request);
        if (!validationResult.IsValid)
        {
            return Result<AddGroupResponse?>.Failure(
                validationResult.Errors.Select(e => new ProblemDetails(e.ErrorMessage, e.ErrorCode)));
        }

        var result = await _groupService.AddGroup(
            request.MemberIds,
            request.OwnerId,
            request.Name,
            request.Description,
            request.CurrencyKey,
            cancellationToken);

        if (!result.IsSuccess || result.Data is null)
        {
            return Result<AddGroupResponse?>.Failure(result.ErrorMessage ?? "Unexpected error occured!");
        }

        return Result<AddGroupResponse?>.Success(new AddGroupResponse { GroupId = result.Data.GroupId });
    }
}

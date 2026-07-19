using Application.Features.Groups.DeleteGroup.Response;
using FluentValidation;
using IOU1.Application.Features.Groups.DeleteGroup.Request;
using IOU1.Application.Services.Groups;
using IOU1.Domain.Models.Results;

namespace IOU1.Application.Features.Groups.DeleteGroup.Handler;

public sealed class DeleteGroupHandler(
    IValidator<DeleteGroupRequest> validator,
    IGroupService groupService)
    : IDeleteGroupHandler
{
    private readonly IValidator<DeleteGroupRequest> _validator = validator;
    private readonly IGroupService _groupService = groupService;

    public async Task<Result<DeleteGroupResponse?>> Handle(DeleteGroupRequest request, CancellationToken cancellationToken = default)
    {
        var validationResult = _validator.Validate(request);
        if (!validationResult.IsValid)
        {
            return Result<DeleteGroupResponse?>.Failure(
                validationResult.Errors.Select(e => new ProblemDetails(e.ErrorMessage, e.ErrorCode)));
        }

        var result = await _groupService.DeleteGroup(request.GroupId, cancellationToken);
        if (!result.IsSuccess)
        {
            return Result<DeleteGroupResponse?>.Failure(result.ErrorMessage ?? "Unexpected error occured!");
        }

        return Result<DeleteGroupResponse?>.Success(new DeleteGroupResponse());
    }
}

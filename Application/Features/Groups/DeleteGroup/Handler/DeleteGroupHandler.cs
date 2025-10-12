using Application.Features.Groups.DeleteGroup.Request;
using Application.Features.Groups.DeleteGroup.Response;
using Application.Service;
using FluentValidation;
using IOU1.Application.Mediator;

namespace Application.Features.Groups.DeleteGroup.Handler;

public class DeleteGroupHandler(IValidator<DeleteGroupRequest> validator, IGroupService groupService) : IRequestHandler<DeleteGroupRequest, DeleteGroupResponse>
{
    private readonly IValidator<DeleteGroupRequest> _validator = validator;
    private readonly IGroupService _groupService = groupService;

    public async Task<DeleteGroupResponse> Handle(DeleteGroupRequest request, CancellationToken cancellationToken = default)
    {
        var validationResult = _validator.Validate(request);
        if (!validationResult.IsValid)
        {
            return new()
            {
                IsSuccess = false,
                ErrorMessage = validationResult.Errors.First().ErrorMessage
            };
        }

        var result = await _groupService.DeleteGroup(request.GroupId, cancellationToken);
        if (!result.IsSuccess)
        {
            return new()
            {
                IsSuccess = false,
                ErrorMessage = result.ErrorMessage
            };
        }

        return new()
        {
            IsSuccess = true
        };
    }
}

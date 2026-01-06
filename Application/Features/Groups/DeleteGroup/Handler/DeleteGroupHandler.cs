using Application.Features.Groups.DeleteGroup.Request;
using Application.Features.Groups.DeleteGroup.Response;
using Application.Service;
using FluentValidation;
using IOU1.Application.Mediator;
using IOU1.Domain.Interfaces;
using MapsterMapper;

namespace IOU1.Application.Features.Groups.DeleteGroup.Handler;

public class DeleteGroupHandler(IValidator<DeleteGroupRequest> validator, IMapper mapper, IGroupService groupService) : RequestHandler<DeleteGroupRequest, DeleteGroupResponse>(validator, mapper)
{
    private readonly IGroupService _groupService = groupService;

    protected override async Task<IResult> Do(DeleteGroupRequest request, CancellationToken cancellationToken = default)
    {
        return await _groupService.DeleteGroup(request.GroupId, cancellationToken);
    }
}

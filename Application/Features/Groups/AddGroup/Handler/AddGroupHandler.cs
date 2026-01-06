using Application.Features.Groups.AddGroup.Request;
using Application.Features.Groups.AddGroup.Response;
using Application.Service;
using FluentValidation;
using IOU1.Application.Mediator;
using IOU1.Domain.Interfaces;
using MapsterMapper;

namespace IOU1.Application.Features.Groups.AddGroup.Handler;

public class AddGroupHandler(IMapper mapper, IValidator<AddGroupRequest> validator, IGroupService groupService)
    : RequestHandler<AddGroupRequest, AddGroupResponse>(validator, mapper)
{
    private readonly IGroupService _groupService = groupService;

    protected override async Task<IResult> Do(AddGroupRequest request, CancellationToken cancellationToken)
    {
        return await _groupService.AddGroup(request.MemberIds, request.OwnerId, request.Description, cancellationToken);
    }
}

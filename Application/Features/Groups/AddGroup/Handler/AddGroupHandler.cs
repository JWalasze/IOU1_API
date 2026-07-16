using FluentValidation;
using IOU1.Application.Features.Groups.AddGroup.Models.Endpoint;
using IOU1.Application.Mediator;
using IOU1.Application.Services.Groups;
using IOU1.Domain.Interfaces;
using MapsterMapper;

namespace IOU1.Application.Features.Groups.AddGroup.Handler;

public class AddGroupHandler(
    IGroupService groupService,
    IValidator<AddGroupRequest> validator,
    IMapper mapper)
    : RequestHandler<AddGroupRequest, AddGroupResponse>(validator, mapper)
{
    private readonly IGroupService _groupService = groupService;

    protected override async Task<IResult> Do(AddGroupRequest request, CancellationToken cancellationToken)
    {
        return await _groupService.AddGroup(
            request.MemberIds,
            request.OwnerId,
            request.Name,
            request.Description,
            request.CurrencyKey,
            cancellationToken);
    }
}

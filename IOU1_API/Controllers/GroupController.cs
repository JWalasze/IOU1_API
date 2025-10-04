using Application.Features.Groups.AddGroup.Request;
using Application.Features.Groups.AddGroup.Response;
using Application.Features.Groups.DeleteGroup.Request;
using Application.Features.Groups.DeleteGroup.Response;
using Application.Features.Groups.GetGroups.Request;
using Application.Features.Groups.GetGroups.Response;
using Application.Mediator;
using IOU1_API.Controllers;
using IOU1_API.Mappers;
using IOU1_API.Services;
using Microsoft.AspNetCore.Mvc;

namespace IOU1.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GroupController(IRequestMediator mediator, xdGroupService groupService) : BaseApiController
{
    private readonly IRequestMediator _mediator = mediator;

    private readonly xdGroupService _groupService = groupService;

    [HttpGet]
    public async Task<IActionResult> GetGroups([FromQuery] GroupsRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send<GroupsRequest, GroupsResponse>(request, cancellationToken);
        return CreateEndpointResponse(result);
    }

    [HttpPost]
    public async Task<IActionResult> AddGroup([FromBody] AddGroupRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send<AddGroupRequest, AddGroupResponse>(request, cancellationToken);
        return CreateEndpointResponse(result);
    }

    [HttpDelete("{GroupId}")]
    public async Task<IActionResult> DeleteGroup([FromRoute] DeleteGroupRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send<DeleteGroupRequest, DeleteGroupResponse>(request, cancellationToken);
        return CreateEndpointResponse(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetGroupWithMembers(long id)
    {
        //var result = await _mediator.Send<GroupsRequest, GroupsResponse>(request, cancellationToken);
        //return CreateEndpointResponse(result);

        var group = await _groupService.GetGroupWithMembersAsync(id);

        if (group == null)
            return NotFound();

        return Ok(group.ToDto());
    }
}

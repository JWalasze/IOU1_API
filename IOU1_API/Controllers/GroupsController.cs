using Application.Features.Groups.DeleteGroup.Response;
using IOU1.Application.Features.Groups.AddGroup.Models.Endpoint;
using IOU1.Application.Features.Groups.DeleteGroup.Request;
using IOU1.Application.Features.Groups.GetGroups.Handler;
using IOU1.Application.Features.Groups.GetGroups.Models.Request;
using IOU1.Application.Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IOU1.API.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class GroupsController : BaseApiController
{
    [HttpGet]
    public async Task<IActionResult> GetGroups(
        [FromQuery] GetGroupsRequest request,
        [FromServices] IGetGroupsHandler handler,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Handle(request, cancellationToken);
        return CreateEndpointResponse(result);
    }

    [HttpPost]
    public async Task<IActionResult> AddGroup(
        [FromBody] AddGroupRequest request,
        [FromServices] IRequestMediator mediator,
        CancellationToken cancellationToken = default)
    {
        var result = await mediator.Send<AddGroupRequest, AddGroupResponse>(request, cancellationToken);
        return CreateEndpointResponse(result);
    }

    [HttpDelete("{GroupId}")]
    public async Task<IActionResult> DeleteGroup(
        [FromRoute] DeleteGroupRequest request,
        [FromServices] IRequestMediator mediator,
        CancellationToken cancellationToken = default)
    {
        var result = await mediator.Send<DeleteGroupRequest, DeleteGroupResponse>(request, cancellationToken);
        return CreateEndpointResponse(result);
    }
}

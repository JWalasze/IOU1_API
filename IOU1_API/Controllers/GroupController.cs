using Application.Features.Groups.Request;
using Application.Features.Groups.Response;
using Application.Mediator;
using Domain.Entities;
using IOU1_API.Mappers;
using IOU1_API.Services;
using Microsoft.AspNetCore.Mvc;

namespace IOU1_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GroupController(IRequestMediator mediator, GroupService groupService) : ControllerBase
{
    private readonly IRequestMediator _mediator = mediator;

    private readonly GroupService _groupService = groupService;

    [HttpGet]
    public async Task<IActionResult> GetGroups([FromQuery] GroupsRequest request)
    {
        return Ok(await _mediator.Send<GroupsRequest, GroupsResponse>(request));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetGroupWithMembers(long id)
    {
        var group = await _groupService.GetGroupWithMembersAsync(id);

        if (group == null)
            return NotFound();

        return Ok(group.ToDto());
    }
}

using Domain.Entities;
using IOU1_API.Mappers;
using IOU1_API.Services;
using Microsoft.AspNetCore.Mvc;

namespace IOU1_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GroupController : ControllerBase
{
    private readonly GroupService _groupService;

    public GroupController(GroupService groupService)
    {
        _groupService = groupService;
    }

    [HttpGet]
    public async Task<IActionResult> GetGroups()
    {
        return Ok();
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

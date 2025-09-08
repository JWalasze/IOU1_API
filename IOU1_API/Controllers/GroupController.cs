using Microsoft.AspNetCore.Mvc;

namespace IOU1_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GroupController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetGroups()
    {
        return Ok();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetGroupMembers(long id)
    {
        return Ok();
    }
}

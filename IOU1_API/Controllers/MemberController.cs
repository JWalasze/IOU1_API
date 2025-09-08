using Microsoft.AspNetCore.Mvc;

namespace IOU1_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MemberController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetMembers()
    {
        return Ok();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetMemberInfo(long id)
    {
        return Ok();
    }
}

using Microsoft.AspNetCore.Mvc;

namespace IOU1_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    public async Task<IActionResult> Login()
    {
        return Ok();
    }
}

using IOU1.Application.Features.Auth.LogIn.Handler;
using IOU1.Application.Features.Auth.LogIn.Models;
using Microsoft.AspNetCore.Mvc;

namespace IOU1.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : BaseApiController
{
    [HttpPost]
    public async Task<IActionResult> Login(
        [FromBody] LogInRequest logInRequest,
        [FromServices] ILogInHandler handler,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Handle(logInRequest, cancellationToken);
        return CreateEndpointResponse(result);
    }
}

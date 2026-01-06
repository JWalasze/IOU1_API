using IOU1.Application.Features.Auth.LogIn.Models;
using IOU1.Application.Mediator;
using Microsoft.AspNetCore.Mvc;

namespace IOU1.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IRequestMediator mediator) : BaseApiController
{
    private readonly IRequestMediator _mediator = mediator;

    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LogInRequest logInRequest, CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send<LogInRequest, LogInResponse>(logInRequest, cancellationToken);
        return CreateEndpointResponse(result);
    }
}

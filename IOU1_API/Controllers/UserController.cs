using Application.Mediator;
using IOU1.Application.Features.Users;
using IOU1_API.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace IOU1.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController(IRequestMediator mediator) : BaseApiController
{
    private readonly IRequestMediator _mediator = mediator;

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] AddUserRequest request)
    {
        var result = await _mediator.Send<AddUserRequest, AddUserResponse>(request);
        return CreateEndpointResponse(result);
    }
}

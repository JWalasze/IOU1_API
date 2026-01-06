using IOU1.Application.Features.Users.AddUser.Models.Endpoint;
using IOU1.Application.Mediator;
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

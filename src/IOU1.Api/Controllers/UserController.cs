using IOU1.Application.Features.Users.AddUser;
using IOU1.Application.Features.Users.AddUser.Models.Endpoint;
using Microsoft.AspNetCore.Mvc;

namespace IOU1.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : BaseApiController
{
    [HttpPost]
    public async Task<IActionResult> Add(
        [FromBody] AddUserRequest request,
        [FromServices] IAddUserHandler handler,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Handle(request, cancellationToken);
        return CreateEndpointResponse(result);
    }
}

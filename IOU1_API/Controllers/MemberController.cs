using IOU1.Application.Features.Members.AddMember.Handler;
using IOU1.Application.Features.Members.AddMember.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IOU1.API.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class MemberController : BaseApiController
{
    [HttpPut]
    public async Task<IActionResult> AddMember(
        [FromBody] AddMemberRequest request,
        [FromServices] IAddMemberHandler handler,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Handle(request, cancellationToken);
        return CreateEndpointResponse(result);
    }
}

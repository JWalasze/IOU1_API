using IOU1.Application.Features.Invitations.GenerateInvitationKey.Handler;
using IOU1.Application.Features.Invitations.GenerateInvitationKey.Request;
using IOU1.Application.Features.Invitations.UseInvitationLink;
using IOU1.Application.Features.Invitations.UseInvitationLink.Models;
using Microsoft.AspNetCore.Mvc;

namespace IOU1.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InvitationController : BaseApiController
{
    [HttpPost("[action]")]
    public async Task<IActionResult> CreateInvitationLink(
        [FromBody] GenerateInvitationKeyRequest request,
        [FromServices] IGenerateInvitationKeyHandler handler,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Handle(request, cancellationToken);
        return CreateEndpointResponse(result);
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> UseInvitationLink(
        [FromBody] UseInvitationLinkRequest request,
        [FromServices] IUseInvitationLinkHandler handler,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Handle(request, cancellationToken);
        return CreateEndpointResponse(result);
    }
}

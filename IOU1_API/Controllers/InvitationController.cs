using IOU1.Application.Features.Invitations.General.GenerateInvitationKey.Handler;
using IOU1.Application.Features.Invitations.General.GenerateInvitationKey.Request;
using IOU1.Application.Features.Invitations.General.UseInvitationLink;
using IOU1.Application.Features.Invitations.General.UseInvitationLink.Models;
using Microsoft.AspNetCore.Mvc;

namespace IOU1.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InvitationController : BaseApiController
{
    [HttpPost("[action]")]
    public async Task<IActionResult> CreateLink(
        [FromBody] GenerateInvitationKeyRequest request,
        [FromServices] IGenerateInvitationKeyHandler handler,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Handle(request, cancellationToken);
        return CreateEndpointResponse(result);
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> AcceptLink(
        [FromBody] UseInvitationLinkRequest request,
        [FromServices] IUseInvitationLinkHandler handler,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Handle(request, cancellationToken);
        return CreateEndpointResponse(result);
    }
}

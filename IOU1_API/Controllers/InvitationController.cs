using IOU1.Application.Features.Invitations.GenerateInvitationKey.Request;
using IOU1.Application.Features.Invitations.UseInvitationLink.Models;
using IOU1.Application.Mediator;
using Microsoft.AspNetCore.Mvc;

namespace IOU1.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InvitationController(IRequestMediator mediator) : BaseApiController
{
    private readonly IRequestMediator _mediator = mediator;

    [HttpPost("[action]")]
    public async Task<IActionResult> CreateInvitationLink([FromBody] GenerateInvitationKeyRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send<GenerateInvitationKeyRequest, UseInvitationLinkResponse>(request, cancellationToken);
        return CreateEndpointResponse(result);
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> UseInvitationLink([FromBody] UseInvitationLinkRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send<UseInvitationLinkRequest, UseInvitationLinkResponse>(request, cancellationToken);
        return CreateEndpointResponse(result);
    }
}

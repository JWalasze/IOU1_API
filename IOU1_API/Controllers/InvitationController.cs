using Application.Mediator;
using IOU1.Application.Features.Invitations.GenerateInvitationKey.Request;
using IOU1.Application.Features.Invitations.GenerateInvitationKey.Response;
using IOU1_API.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace IOU1.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InvitationController(IRequestMediator mediator) : BaseApiController
{
    private readonly IRequestMediator _mediator = mediator;

    [HttpPost]
    public async Task<IActionResult> CreateInvitationLink([FromBody] GenerateInvitationKeyRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send<GenerateInvitationKeyRequest, GenerateInvitationKeyResponse>(request, cancellationToken);
        return CreateEndpointResponse(result);
    }

}

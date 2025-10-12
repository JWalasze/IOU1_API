using Application.Features.Groups.AddGroup.Request;
using Application.Mediator;
using IOU1.Application.Features.Invitations.DirectInvitation.Models;
using IOU1_API.Controllers;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;

namespace IOU1.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DirectInvitationController(IRequestMediator mediator) : BaseApiController
    {
        private readonly IRequestMediator _mediator = mediator;
        [HttpPost]
        public async Task<IActionResult> CreateInvitation([FromBody] DirectInvitationCreationRequest request, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send<DirectInvitationCreationRequest, DirectInvitationCreationResponse>(request, cancellationToken);
            return CreateEndpointResponse(result);
        }
    }
}

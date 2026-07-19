using IOU1.Application.Features.Invitations.DirectInvitation;
using IOU1.Application.Features.Invitations.DirectInvitation.Models;
using Microsoft.AspNetCore.Mvc;

namespace IOU1.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DirectInvitationController : BaseApiController
    {
        [HttpPost]
        public async Task<IActionResult> CreateInvitation(
            [FromBody] DirectInvitationCreationRequest request,
            [FromServices] IDirectInvitationCreationHandler handler,
            CancellationToken cancellationToken = default)
        {
            var result = await handler.Handle(request, cancellationToken);
            return CreateEndpointResponse(result);
        }
    }
}

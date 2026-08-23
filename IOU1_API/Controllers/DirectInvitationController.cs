using IOU1.Application.Features.Invitations.Direct.AddDirectInvitation.Handler;
using IOU1.Application.Features.Invitations.Direct.AddDirectInvitation.Models.Request;
using IOU1.Application.Features.Invitations.Direct.UseDirectInvitation.Handler;
using IOU1.Application.Features.Invitations.Direct.UseDirectInvitation.Models.Request;
using Microsoft.AspNetCore.Mvc;

namespace IOU1.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DirectInvitationController : BaseApiController
    {
        [HttpPost("[action]")]
        public async Task<IActionResult> Create(
            [FromBody] AddDirectInvitationRequest request,
            [FromServices] IAddDirectInvitationHandler handler,
            CancellationToken cancellationToken = default)
        {
            var result = await handler.Handle(request, cancellationToken);
            return CreateEndpointResponse(result);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Accept(
            [FromBody] UseDirectInvitationRequest request,
            [FromServices] IUseDirectInvitationHandler handler,
            CancellationToken cancellationToken = default)
        {
            var result = await handler.Handle(request, cancellationToken);
            return CreateEndpointResponse(result);
        }
    }
}

using IOU1.Application.Features.Expenses.Options.GetExpenseOptiions.Handler;
using IOU1.Application.Features.Expenses.Options.GetExpenseOptions.Models.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IOU1.API.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class ExpenseOptionsController : BaseApiController
{
    [HttpGet]
    public async Task<IActionResult> Get(
        [FromServices] IGetExpenseOptionsHandler handler,
        [FromQuery] GetExpenseOptionsRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Handle(request, cancellationToken);
        return CreateEndpointResponse(result);
    }
}

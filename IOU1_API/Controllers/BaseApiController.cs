using IOU1.Application;
using IOU1.Application.Mediator;
using Microsoft.AspNetCore.Mvc;

namespace IOU1.API.Controllers;

public class BaseApiController : ControllerBase
{
    [NonAction]
    protected IActionResult CreateEndpointResponse(IHandlerResponse<IResponse> response)
    {
        //TODO Logic if there is a failure
        return Ok(response);
    }
}

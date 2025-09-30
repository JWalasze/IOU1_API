using Application.Mediator;
using Microsoft.AspNetCore.Mvc;

namespace IOU1_API.Controllers;

public class BaseApiController : ControllerBase
{
    [NonAction]
    protected IActionResult CreateEndpointResponse(IResponse response)
    {
        //TODO Logic if there is a failure
        return Ok(response);
    }
}

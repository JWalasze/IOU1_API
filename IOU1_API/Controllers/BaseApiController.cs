using Application.Mediator;
using Microsoft.AspNetCore.Mvc;

namespace IOU1_API.Controllers;

public class BaseApiController : ControllerBase
{
    [NonAction]
    protected IActionResult CreateEndpointResponse(IResponse response)
    {
        return Ok(response);
    }
}

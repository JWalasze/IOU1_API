using Application.Mediator;
using Microsoft.AspNetCore.Mvc;

namespace IOU1_API.Controllers;

public class BaseApiController : ControllerBase
{
    public IActionResult CreateEndpointResponse(IResponse response)
    {
        return Ok(response);
    }
}

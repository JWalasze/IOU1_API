using IOU1.Domain.Models.Results;
using Microsoft.AspNetCore.Mvc;

namespace IOU1.API.Controllers;

public abstract class BaseApiController : ControllerBase
{
    [NonAction]
    protected IActionResult CreateEndpointResponse<T>(Result<T> result) where T : class?
    {
        if (result.IsSuccess)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }
}

using IOU1.Application;
using IOU1.Application.Mediator;
using IOU1.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace IOU1.API.Controllers;

public abstract class BaseApiController : ControllerBase
{
    [NonAction]
    protected IActionResult CreateEndpointResponse(IHandlerResponse<IResponse> response)
    {
        var errorList = response.Errors.ToList();
        if (errorList.Count == 0)
        {
            return Ok(response);
        }

        if (errorList.Count == 1)
        {
            var problemDetail = errorList[0];
            //TODO Create a singleton service which will hold error code mapped to the endpoint problem details
            return NotFound(problemDetail);
        }

        return BadRequest(response.Errors);
    }

    [NonAction]
    protected IActionResult CreateEndpointResponse<T>(Result<T> result) where T : class
    {
        if (result.IsSuccess)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }
}

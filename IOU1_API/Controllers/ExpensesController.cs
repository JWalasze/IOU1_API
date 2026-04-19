using IOU1.Application.Features.Expenses.AddExpense.Handler;
using IOU1.Application.Features.Expenses.AddExpense.Models;
using IOU1.Application.Features.Expenses.GetExpenses.Handler;
using IOU1.Application.Features.Expenses.GetExpenses.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IOU1.API.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class ExpensesController : BaseApiController
{
    [HttpPost]
    public async Task<IActionResult> AddExpense(
        [FromServices] IAddExpenseHandler handler,
        [FromBody] AddExpenseRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Handle(request, cancellationToken);
        return CreateEndpointResponse(result);
    }

    [HttpGet("{GroupId:long}")]
    public async Task<IActionResult> GetGroupExpenses(
        [FromRoute] GetExpensesRequest request,
        [FromServices] IGetExpensesHandler handler,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Handle(request, cancellationToken);
        return CreateEndpointResponse(result);
    }
}

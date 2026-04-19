using FluentValidation;
using IOU1.Application.Features.Expenses.GetExpenses.Models;
using IOU1.Application.Services.Expenses;
using IOU1.Domain.Models.Results;
using Mapster;

namespace IOU1.Application.Features.Expenses.GetExpenses.Handler;

public class GetExpensesHandler(
    IValidator<GetExpensesRequest> validator,
    IExpenseService expenseService) : IGetExpensesHandler
{
    private readonly IValidator<GetExpensesRequest> _validator = validator;
    private readonly IExpenseService _expenseService = expenseService;

    public async Task<Result<GetExpensesResponse?>> Handle(GetExpensesRequest request, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return Result<GetExpensesResponse?>.Failure(
               validationResult.Errors.Select(e => new ProblemDetails(e.ErrorMessage, e.ErrorCode)));
        }

        var summary = await _expenseService.GetSummary(request.GroupId, cancellationToken);
        if (!summary.IsSuccess)
        {
            return Result<GetExpensesResponse?>.Failure(summary.Errors);
        }

        var response = summary.Data.Adapt<GetExpensesResponse>();
        return Result<GetExpensesResponse?>.Success(response);
    }
}

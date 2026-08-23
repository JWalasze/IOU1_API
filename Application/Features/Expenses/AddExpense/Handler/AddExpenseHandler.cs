using FluentValidation;
using IOU1.Application.Features.Expenses.AddExpense.Models;
using IOU1.Application.Features.Expenses.AddExpense.Models.Request;
using IOU1.Application.Features.Expenses.AddExpense.Models.Response;
using IOU1.Application.Services.Expenses;
using IOU1.Domain.Models;
using IOU1.Domain.Models.Results;
using IOU1.Domain.UnitOfWork;

namespace IOU1.Application.Features.Expenses.AddExpense.Handler;

public class AddExpenseHandler(
    IValidator<AddExpenseRequest> validator,
    IUnitOfWork unitOfWork,
    IExpenseService expenseService) : IAddExpenseHandler
{
    private readonly IValidator<AddExpenseRequest> _validator = validator;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IExpenseService _expenseService = expenseService;

    public async Task<Result<AddExpenseResponse?>> Handle(AddExpenseRequest request, CancellationToken cancellationToken = default)
    {
        var validationResult = _validator.Validate(request);
        if (!validationResult.IsValid)
            return Result<AddExpenseResponse?>.Failure(validationResult.Errors);

        await _unitOfWork.BeginTransaction();

        try
        {
            var newExpense = new NewExpense(
                request.Title,
                request.Description,
                request.PayerId,
                request.GroupId,
                request.Amount,
                request.SplitTypeId,
                request.CategoryId,
                request.Splits.Select(s => new Split
                {
                    Amount = s.Amount,
                    MemberId = s.MemberId,
                    Percentage = s.Percentage
                }));

            var result = await _expenseService.AddExpense(
                newExpense,
                cancellationToken);

            if (!result.IsSuccess || result.Data is null)
            {
                await _unitOfWork.RollbackTransaction();
                return Result<AddExpenseResponse?>.Failure(result.ErrorMessage ?? "We had problem adding your expense. Try again later.");
            }

            var expense = result.Data;
            var response = new AddExpenseResponse(expense.Id);

            await _unitOfWork.CommitTransaction();
            return Result<AddExpenseResponse?>.Success(response);
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransaction();
            return Result<AddExpenseResponse?>.Failure(ex.Message);
        }
    }
}

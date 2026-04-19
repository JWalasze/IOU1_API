using FluentValidation;
using IOU1.Application.Features.Expenses.AddExpense.Models;
using IOU1.Application.Services.Expenses;
using IOU1.Application.Services.Members.Debts;
using IOU1.Domain.Models;
using IOU1.Domain.Models.Results;
using IOU1.Domain.UnitOfWork;

namespace IOU1.Application.Features.Expenses.AddExpense.Handler;

public class AddExpenseHandler(
    IValidator<AddExpenseRequest> validator,
    IExpenseService expenseService,
    IMemberDebtService memberDebtService,
    IUnitOfWork unitOfWork) : IAddExpenseHandler
{
    private readonly IValidator<AddExpenseRequest> _validator = validator;
    private readonly IExpenseService _expenseService = expenseService;
    private readonly IMemberDebtService _memberDebtService = memberDebtService;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<AddExpenseResponse?>> Handle(AddExpenseRequest request, CancellationToken cancellationToken = default)
    {
        await _unitOfWork.BeginTransaction();

        try
        {
            var validationResult = _validator.Validate(request);
            if (!validationResult.IsValid)
            {
                return Result<AddExpenseResponse?>.Failure(
                    validationResult.Errors.Select(e => new ProblemDetails(e.ErrorMessage, e.ErrorCode)));
            }

            var newExpense = new NewExpense(
                request.BuyerId,
                request.GroupId,
                request.Amount,
                request.Title,
                request.Description,
                request.SplitType,
                request.Splits.Select(s => new Split { Amount = s.Amount, MemberId = s.MemberId }));

            var expense = await _expenseService.AddExpense(
                newExpense,
                cancellationToken);

            if (!expense.IsSuccess || expense.Data is null)
            {
                return Result<AddExpenseResponse?>.Failure(expense.ErrorMessage ?? "Unexpected error occured!");
            }

            var updateDebtsResult = await _memberDebtService.UpdateMemberDebts(expense.Data, cancellationToken);
            if (!updateDebtsResult.IsSuccess)
            {
                return Result<AddExpenseResponse?>.Failure(updateDebtsResult.ErrorMessage ?? "Unexpected error occured while updating member debts!");
            }

            //var response = expense.Data.Adapt<AddExpenseResponse>();
            var response = new AddExpenseResponse();

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

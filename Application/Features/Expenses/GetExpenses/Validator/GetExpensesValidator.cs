using FluentValidation;
using IOU1.Application.Features.Expenses.GetExpenses.Models;

namespace IOU1.Application.Features.Expenses.GetExpenses.Validator;

public class GetExpensesValidator : AbstractValidator<GetExpensesRequest>
{
    public GetExpensesValidator()
    {

    }
}

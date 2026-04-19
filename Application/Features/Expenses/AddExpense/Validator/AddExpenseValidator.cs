using FluentValidation;
using IOU1.Application.Features.Expenses.AddExpense.Models;

namespace IOU1.Application.Features.Expenses.AddExpense.Validator;

public class AddExpenseValidator : AbstractValidator<AddExpenseRequest>
{
    public AddExpenseValidator()
    {

    }
}

using FluentValidation;
using IOU1.Application.Features.Expenses.Options.GetExpenseOptions.Models.Request;

namespace IOU1.Application.Features.Expenses.Options.GetExpenseOptions.Validator;

public sealed class GetExpenseOptionsValidator : AbstractValidator<GetExpenseOptionsRequest>
{
    public GetExpenseOptionsValidator()
    {
        RuleFor(eo => eo.GroupId)
            .GreaterThan(0)
            .WithErrorCode("INVALID_GROUP_ID_ERROR")
            .WithMessage($"Group ID must be greater than zero!");
    }
}

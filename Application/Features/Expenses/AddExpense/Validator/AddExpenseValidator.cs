using FluentValidation;
using IOU1.Application.Features.Expenses.AddExpense.Models.Request;
using IOU1.Domain.Constants.Expense;
using IOU1.Domain.Entities;

namespace IOU1.Application.Features.Expenses.AddExpense.Validator;

public class AddExpenseValidator : AbstractValidator<AddExpenseRequest>
{
    public AddExpenseValidator()
    {
        RuleFor(e => e.Title)
            .NotEmpty()
            .WithErrorCode("EMPTY_EXPENSE_TITLE_ERROR")
            .WithMessage("The expense title cannot be empty.")
            .MaximumLength(Expense.MaxTitleLength)
            .WithErrorCode("TOO_LONG_EXPENSE_TITLE_ERROR")
            .WithMessage($"The expense title is too long. Limit: {Expense.MaxTitleLength}.");

        RuleFor(e => e.Description)
            .MaximumLength(Expense.MaxDescriptionLength)
            .WithErrorCode("TOO_LONG_EXPENSE_DESCRIPTION_ERROR")
            .WithMessage($"The expense description is too long. Limit: {Expense.MaxDescriptionLength}.")
            .When(ag => !string.IsNullOrWhiteSpace(ag.Description));

        RuleFor(e => e.PayerId)
            .GreaterThan(0)
            .WithErrorCode("INVALID_PAYER_ID_ERROR")
            .WithMessage($"The payer id must be greater than 0.");

        RuleFor(e => e.GroupId)
            .GreaterThan(0)
            .WithErrorCode("INVALID_GROUP_ID_ERROR")
            .WithMessage($"The group id must be greater than 0.");

        RuleFor(e => e.SplitTypeId)
            .GreaterThan(0)
            .WithErrorCode("INVALID_SPLIT_TYPE_ID_ERROR")
            .WithMessage($"The split type id must be greater than 0.");

        RuleFor(e => e.CategoryId)
            .GreaterThan(0)
            .WithErrorCode("INVALID_CATEGORY_ID_ERROR")
            .WithMessage($"The category id must be greater than 0.");

        RuleFor(e => e.Amount)
            .GreaterThan(0)
            .WithErrorCode("INVALID_AMOUNT_ERROR")
            .WithMessage($"The amount must be greater than 0.");

        RuleFor(e => e.Splits)
            .Must(splits => splits.All(x => x.MemberId > 0))
            .WithErrorCode("INVALID_SPLIT_MEMBER_ID_ERROR")
            .WithMessage($"The member ids must be greater than 0.")
            .Must(splits => splits.All(x => x.Amount > 0))
            .WithErrorCode("INVALID_SPLIT_AMOUNT_ERROR")
            .WithMessage($"The split amount must be greater than 0.")
            .When(e => e.Splits.Any());

        RuleFor(e => e.Splits)
            .NotEmpty()
            .WithErrorCode("EMPTY_SPLITS_ERROR")
            .WithMessage($"The splits cannot be empty for the chosen split id.");

        RuleFor(e => e.Splits)
            .Must(splits => splits.All(x => x.Percentage > 0 && x.Percentage <= 100))
            .WithErrorCode("INVALID_SPLITS_PERCENTAGE_ERROR")
            .WithMessage($"The percentages must be between 0 and 100.")
            .When(e => e.SplitTypeId == SplitTypeId.Percentage);

        RuleFor(e => e.Splits)
            .Must(splits => splits.All(x => x.Percentage is null))
            .WithErrorCode("CONFLICT_SPLITS_PERCENTAGE_ERROR")
            .WithMessage($"Besides percentages no other split types can have percentages.")
            .When(e => e.SplitTypeId != SplitTypeId.Percentage);
    }
}

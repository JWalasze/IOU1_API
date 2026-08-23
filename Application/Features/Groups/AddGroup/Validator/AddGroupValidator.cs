using FluentValidation;
using IOU1.Application.Features.Groups.AddGroup.Models.Endpoint;
using IOU1.Domain.Entities;

namespace IOU1.Application.Features.Groups.AddGroup.Validator;

public sealed class AddGroupValidator : AbstractValidator<AddGroupRequest>
{
    public AddGroupValidator()
    {
        RuleFor(ag => ag.Name)
            .NotEmpty()
            .WithErrorCode("EMPTY_GROUP_NAME_ERROR")
            .WithMessage("The name of the group cannot be empty.")
            .MaximumLength(Group.NameMaxLength)
            .WithErrorCode("MAX_LENGTH_GROUP_NAME_ERROR")
            .WithMessage($"The name of group is too long. Max: {Group.NameMaxLength} chars.");

        RuleFor(ag => ag.Description)
            .MaximumLength(Group.DescriptionMaxLength)
            .WithErrorCode("MAX_LENGTH_DESCRIPTION_ERROR")
            .WithMessage($"The description is too long. Max: {Group.DescriptionMaxLength} chars.")
            .When(ag => !string.IsNullOrWhiteSpace(ag.Description));

        RuleFor(ag => ag.CurrencyKey)
            .NotEmpty()
            .WithErrorCode("EMPTY_GROUP_CURRENCY_KEY_ERROR")
            .WithMessage("CurrencyKey cannot be empty.");
    }
}

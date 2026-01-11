using FluentValidation;
using IOU1.Application.Features.Groups.AddGroup.Models.Endpoint;
using IOU1.Domain.Entities;

namespace IOU1.Application.Features.Groups.AddGroup.Validator;

public class AddGroupValidator : AbstractValidator<AddGroupRequest>
{
    public AddGroupValidator()
    {
        RuleFor(ag => ag.MemberIds)
            .NotEmpty()
            .WithErrorCode("EMPTY_GROUP_MEMBER_IDS_ERROR")
            .WithMessage("MemberIds cannot be empty.")
            .Must(ids => ids.All(id => id > 0))
            .WithErrorCode("INVALID_GROUP_MEMBER_IDS_ERROR")
            .WithMessage("All MemberIds must be positive long values.");

        RuleFor(ag => ag.Name)
            .NotEmpty()
            .WithErrorCode("EMPTY_GROUP_NAME_ERROR")
            .WithMessage("The name of the group cannot be empty.")
            .MaximumLength(Group.NameMaxLength)
            .WithErrorCode("MAX_LENGTH_GROUP_NAME_ERROR")
            .WithMessage($"The name of group is too long. Max: {Group.NameMaxLength} chars.");

        RuleFor(ag => ag.Description)
            .MaximumLength(Group.DescMaxLength)
            .WithErrorCode("MAX_LENGTH_DESC_ERROR")
            .WithMessage($"The description is too long. Max: {Group.DescMaxLength} chars.");
    }
}

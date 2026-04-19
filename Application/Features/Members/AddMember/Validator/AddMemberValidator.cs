using FluentValidation;
using IOU1.Application.Features.Members.AddMember.Models;

namespace IOU1.Application.Features.Members.AddMember.Validator;

public class AddMemberValidator : AbstractValidator<AddMemberRequest>
{
    public AddMemberValidator()
    {
        RuleFor(am => am.UserId)
            .GreaterThan(0)
            .WithErrorCode("INVALID_USER_ID_NUMBER_ERROR")
            .WithMessage($"User ID must be greater than zero!");

        RuleFor(am => am.GroupId)
            .GreaterThan(0)
            .WithErrorCode("INVALID_GROUP_ID_NUMBER_ERROR")
            .WithMessage($"Group ID must be greater than zero!");
    }
}

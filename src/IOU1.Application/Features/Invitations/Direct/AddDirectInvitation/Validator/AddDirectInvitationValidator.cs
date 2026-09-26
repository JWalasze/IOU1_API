using FluentValidation;
using IOU1.Application.Features.Invitations.Direct.AddDirectInvitation.Models.Request;

namespace IOU1.Application.Features.Invitations.Direct.AddDirectInvitation.Validator;

public class AddDirectInvitationValidator : AbstractValidator<AddDirectInvitationRequest>
{
    public AddDirectInvitationValidator()
    {
        RuleFor(req => req.Email)
            .NotEmpty()
            .WithErrorCode("INVITATION_EMAIL_ERROR")
            .WithMessage("Email address cannot be empty.")
            .EmailAddress()
            .WithMessage("Email address is not in correct format.");

        RuleFor(req => req.GroupId)
            .NotEmpty()
            .WithErrorCode("INVITATION_GROUP_ID_ERROR")
            .WithMessage("Group ID is missing.");
    }
}

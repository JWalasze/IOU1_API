using FluentValidation;
using IOU1.Application.Features.Invitations.Direct.UseDirectInvitation.Models.Request;

namespace IOU1.Application.Features.Invitations.Direct.UseDirectInvitation.Validator;

public sealed class UseDirectInvitationValidator : AbstractValidator<UseDirectInvitationRequest>
{
    public UseDirectInvitationValidator()
    {
        RuleFor(udi => udi.InvitationId)
            .NotEmpty()
            .WithErrorCode("INVITATION_MISSING_ID_ERROR")
            .WithMessage("Invitation ID is missing!");
    }
}

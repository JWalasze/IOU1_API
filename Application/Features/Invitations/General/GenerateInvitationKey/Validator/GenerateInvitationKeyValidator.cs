using FluentValidation;
using IOU1.Application.Features.Invitations.General.GenerateInvitationKey.Request;

namespace IOU1.Application.Features.Invitations.General.GenerateInvitationKey.Validator;

public class GenerateInvitationKeyValidator : AbstractValidator<GenerateInvitationKeyRequest>
{
    public GenerateInvitationKeyValidator()
    {

    }
}

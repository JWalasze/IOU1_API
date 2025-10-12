using FluentValidation;
using IOU1.Application.Features.Invitations.GenerateInvitationKey.Request;

namespace IOU1.Application.Features.Invitations.GenerateInvitationKey.Validator;

public class GenerateInvitationKeyValidator : AbstractValidator<GenerateInvitationKeyRequest>
{
    public GenerateInvitationKeyValidator()
    {

    }
}

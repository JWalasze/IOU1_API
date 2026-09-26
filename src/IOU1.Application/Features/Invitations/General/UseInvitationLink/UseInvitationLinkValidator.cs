using FluentValidation;
using IOU1.Application.Features.Invitations.General.UseInvitationLink.Models;
using IOU1.Persistance.Context;
using Microsoft.EntityFrameworkCore;

namespace IOU1.Application.Features.Invitations.General.UseInvitationLink;

public class UseInvitationLinkValidator : AbstractValidator<UseInvitationLinkRequest>
{
    public UseInvitationLinkValidator(IOU1Context dbContext)
    {
        RuleFor(x => x)
            .Custom((request, context) =>
            {
                if (request.InvitationId is null && request.InvitationKey is null)
                {
                    context.AddFailure("Either InvitationId or InvitationKey must be provided.");
                }
                else if (request.InvitationId is not null && request.InvitationKey is not null)
                {
                    context.AddFailure("Only one of InvitationId or InvitationKey should be provided, not both.");
                }
            });

        RuleFor(x => x)
            .CustomAsync(async (request, context, ct) =>
            {
                if (request.InvitationId is not null)
                {
                    //TODO Logic for checking if the invitation id exists
                }

                if (request.InvitationKey is not null)
                {
                    var invitationExists = await dbContext.InvitationLinks
                        .AsNoTracking()
                        .AnyAsync(i => i.InvitationKey.Key == request.InvitationKey, ct);

                    if (!invitationExists)
                    {
                        context.AddFailure("InvitationKey", "The provided InvitationKey does not exist.");
                    }
                }
            });
    }
}

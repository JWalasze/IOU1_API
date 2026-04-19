using FluentValidation;
using IOU1.Application.Features.Invitations.DirectInvitation.Models;
using IOU1.Persistance.Context;
using Microsoft.EntityFrameworkCore;

namespace IOU1.Application.Features.Invitations.DirectInvitation;

public class DirectInvitationCreationValidator: AbstractValidator<DirectInvitationCreationRequest>
{
    private readonly IOU1Context _context;

    public DirectInvitationCreationValidator(IOU1Context context)
    {
        _context = context;

        RuleFor(req => req.email)
            .NotEmpty().WithMessage("Email cannot be empty.")
            .EmailAddress().WithMessage("Wrong email format.");

        RuleFor(req => req.SenderId)
            .NotEmpty().WithMessage("Sender cannot be empty.");

        RuleFor(req => req.GroupId)
            .NotEmpty().WithMessage("Group cannot be empty.");

        RuleFor(req => req)
            .MustAsync(async (model, ct) =>
            {
                // Check if the group exists AND contains the sender as a member
                /* test data:
                {
                    "groupId": 4,
                    "senderId": 3, 
                    "email": "alice.johnson@example.com"
                }
                 
                 */
                var exists = await _context.Groups
                    .AnyAsync(g =>
                        g.Id == model.GroupId &&
                        g.Members.Any(m => m.UserId == model.SenderId),
                        ct);

                return exists;
            })
            .WithMessage("Sender is not in the group!");

    }
}

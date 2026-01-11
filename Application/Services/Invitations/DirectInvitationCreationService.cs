using IOU1.Domain.Entities;
using IOU1.Domain.Exceptions;
using IOU1.Persistance.Context;
using Microsoft.EntityFrameworkCore;

namespace IOU1.Application.Services.Invitations;

public record UserDto(string Name);

public class DirectInvitationCreationService(IOU1Context context) : IDirectInvitationCreationService
{
    IOU1Context _context = context;

    public async Task<UserDto?> CheckInvitationPossible(string email, long groupId, long senderId)
    {
        throw new NotImplementedException();
    }

    public async Task<Invitation> MakeInvitation(string email, long groupId, long senderId)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email.EmailAddress == email);

        if (user == null)
        {
            throw new UserNotFoundException();
        }

        Invitation invitation = new(groupId, user.Id, senderId);
        await _context.Invitations.AddAsync(invitation);
        await _context.SaveChangesAsync();

        return invitation;
    }
}

using IOU1.Domain.Entities;

namespace IOU1.Application.Services.Invitations;

public interface IDirectInvitationCreationService
{
    Task<Invitation> MakeInvitation(string email, long groupId, long senderId);
    Task<UserDto?> CheckInvitationPossible(string email, long groupId, long senderId);

}

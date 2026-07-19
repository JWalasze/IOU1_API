using IOU1.Domain.Entities;

namespace IOU1.Application.Services.Invitations;

public interface IDirectInvitationCreationService
{
    Task<Invitation> MakeInvitation(string email, int groupId, int senderId);
    Task<UserDto?> CheckInvitationPossible(string email, int groupId, int senderId);

}

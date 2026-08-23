using FluentValidation;
using IOU1.Application.Features.Invitations.Direct.AddDirectInvitation.Models.Request;
using IOU1.Application.Features.Invitations.Direct.AddDirectInvitation.Models.Response;
using IOU1.Application.Services.Notifications;
using IOU1.Domain.Entities;
using IOU1.Domain.Entities.Notifications;
using IOU1.Domain.Enums;
using IOU1.Domain.Models.Auth.User;
using IOU1.Domain.Models.Results;
using IOU1.Domain.UnitOfWork;
using IOU1.Persistance.Context;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace IOU1.Application.Features.Invitations.Direct.AddDirectInvitation.Handler;

public sealed class AddDirectInvitationHandler(
    IValidator<AddDirectInvitationRequest> validator,
    IAuthUser user,
    IUnitOfWork unit,
    IOU1Context context,
    INotificationService notificationService)
    : IAddDirectInvitationHandler
{
    private readonly IValidator<AddDirectInvitationRequest> _validator = validator;
    private readonly IAuthUser _user = user;
    private readonly IUnitOfWork _unit = unit;
    private readonly IOU1Context _context = context;
    private readonly INotificationService _notificationService = notificationService;

    public async Task<Result<AddDirectInvitationResponse?>> Handle(AddDirectInvitationRequest request, CancellationToken cancellationToken = default)
    {
        var validationResult = _validator.Validate(request);
        if (!validationResult.IsValid)
            return Result<AddDirectInvitationResponse?>.Failure(validationResult.Errors);

        try
        {
            await _unit.BeginTransaction();

            var userToBeAdded = await _context.Users
                .Where(u => u.Email.EmailAddress == request.Email)
                .FirstOrDefaultAsync(cancellationToken);

            if (userToBeAdded is null)
                return Result<AddDirectInvitationResponse?>.Failure("User with provided email address doesn't exist.");

            var groupMember = await _context.GroupMembers
                .Include(u => u.User)
                .Include(u => u.Group)
                .Where(u => u.UserId == _user.Id)
                .FirstOrDefaultAsync(cancellationToken);

            if (groupMember is null)
                return Result<AddDirectInvitationResponse?>.Failure("Internal server error. Try again later.");

            var invitation = Invitation.Create(
                groupMember.GroupId,
                userToBeAdded.Id,
                senderId: _user.Id);

            await _context.Invitations.AddAsync(invitation, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            var title = "Zaproszenie do nowej grupy";
            var message = @$"Użytkownik {groupMember.User.Login} zaprosił cię do grupy {groupMember.Group.Name}!
                Czy akceptujesz zaproszenie?";

            var dbPayload = new
            {
                InvitationId = invitation.Id,
                Title = title,
                Message = message
            };

            var serializedDbPayload = JsonSerializer.Serialize(dbPayload);
            var notification = Notification.Create(
                createdAt: DateTime.UtcNow,
                payload: serializedDbPayload,
                userId: userToBeAdded.Id,
                type: NotificationType.DirectInvitationToGroup);

            await _context.Notifications.AddAsync(notification, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            var signalPayload = new
            {
                InvitationId = invitation.Id,
                NotificationId = notification.Id,
                Title = title,
                Message = message
            };
            await _notificationService.SendToUser(userToBeAdded.Id, serializedDbPayload, cancellationToken);

            return Result<AddDirectInvitationResponse?>.Success(new AddDirectInvitationResponse());
        }
        catch (Exception ex)
        {
            await _unit.RollbackTransaction();
            return Result<AddDirectInvitationResponse?>.Failure(ex, "An unexpected error occured while creating an invitation!");
        }
    }
}

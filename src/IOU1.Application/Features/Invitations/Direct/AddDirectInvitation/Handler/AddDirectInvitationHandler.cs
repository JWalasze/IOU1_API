using System.Text.Json;
using FluentValidation;
using IOU1.Application.Features.Invitations.Direct.AddDirectInvitation.Models.Request;
using IOU1.Application.Features.Invitations.Direct.AddDirectInvitation.Models.Response;
using IOU1.Application.Persistance;
using IOU1.Application.Services.Notifications;
using IOU1.Domain.Entities;
using IOU1.Domain.Entities.Notifications;
using IOU1.Domain.Enums;
using IOU1.Domain.Models.Auth.User;
using IOU1.Domain.Models.Results;
using IOU1.Persistance.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IOU1.Application.Features.Invitations.Direct.AddDirectInvitation.Handler;

public sealed class AddDirectInvitationHandler(
    IValidator<AddDirectInvitationRequest> validator,
    IAuthUser user,
    IUnitOfWork unit,
    IOU1Context context,
    ILogger<AddDirectInvitationHandler> logger,
    INotificationService notificationService
) : IAddDirectInvitationHandler
{
    private readonly IValidator<AddDirectInvitationRequest> _validator = validator;
    private readonly IAuthUser _user = user;
    private readonly IUnitOfWork _unit = unit;
    private readonly IOU1Context _context = context;
    private readonly ILogger<AddDirectInvitationHandler> _logger = logger;
    private readonly INotificationService _notificationService = notificationService;

    public async Task<Result<AddDirectInvitationResponse?>> Handle(
        AddDirectInvitationRequest request,
        CancellationToken cancellationToken = default
    )
    {
        var validationResult = _validator.Validate(request);
        if (!validationResult.IsValid)
            return Result<AddDirectInvitationResponse?>.Failure(validationResult.Errors);

        User? userToBeAdded = null;
        Invitation? invitation = null;
        Notification? notification = null;
        string? serializedDbPayload = null;

        try
        {
            await _unit.BeginTransaction();

            userToBeAdded = await _context
                .Users.Where(u => u.Email.EmailAddress == request.Email)
                .FirstOrDefaultAsync(cancellationToken);

            if (userToBeAdded is null)
                return Result<AddDirectInvitationResponse?>.Failure(
                    "User with provided email address doesn't exist."
                );

            var groupMember = await _context
                .GroupMembers.Include(u => u.User)
                .Include(u => u.Group)
                .Where(u => u.UserId == _user.Id)
                .FirstOrDefaultAsync(cancellationToken);

            if (groupMember is null)
                return Result<AddDirectInvitationResponse?>.Failure(
                    "Internal server error. Try again later."
                );

            invitation = Invitation.Create(
                groupMember.GroupId,
                userToBeAdded.Id,
                senderId: _user.Id
            );

            await _context.Invitations.AddAsync(invitation, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            var title = "Zaproszenie do nowej grupy";
            var message =
                @$"Użytkownik {groupMember.User.Login} zaprosił cię do grupy {groupMember.Group.Name}!
                Czy akceptujesz zaproszenie?";

            var dbPayload = new
            {
                InvitationId = invitation.Id,
                Title = title,
                Message = message,
            };

            serializedDbPayload = JsonSerializer.Serialize(dbPayload);
            notification = Notification.Create(
                createdAt: DateTime.UtcNow,
                payload: serializedDbPayload,
                userId: userToBeAdded.Id,
                type: NotificationType.InvitationToGroup
            );

            await _context.Notifications.AddAsync(notification, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            var signalPayload = new
            {
                InvitationId = invitation.Id,
                NotificationId = notification.Id,
                Title = title,
                Message = message,
            };

            await _unit.CommitTransaction();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occured while creating an invitation!");
            await _unit.RollbackTransaction();
            return Result<AddDirectInvitationResponse?>.Failure(
                ex,
                $"An unexpected error occured while creating an invitation for email {request.Email}!"
            );
        }

        //Moze notyfikacje powinny zwracac result, bo moze sie nie udac wyslac powiadomienia, ale zaproszenie zostanie dodane do bazy danych
        try
        {
            await _notificationService.SendToUser(
                userToBeAdded.Id,
                notification.Id,
                serializedDbPayload,
                cancellationToken
            );
            return Result<AddDirectInvitationResponse?>.Success(
                new AddDirectInvitationResponse(invitation.Id, notification.Id)
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "An unexpected error occured while sending a notification to user with id {UserId}!",
                userToBeAdded.Id
            );
            return Result<AddDirectInvitationResponse?>.Failure(
                ex,
                $"An unexpected error occured while sending a notification to user with id {userToBeAdded.Id}!"
            );
        }
    }
}

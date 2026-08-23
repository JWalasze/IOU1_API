using FluentValidation;
using IOU1.Application.Features.Invitations.Direct.UseDirectInvitation.Models.Request;
using IOU1.Application.Features.Invitations.Direct.UseDirectInvitation.Models.Response;
using IOU1.Application.Services.Members;
using IOU1.Domain.Enums;
using IOU1.Domain.Models.Auth.User;
using IOU1.Domain.Models.Results;
using IOU1.Domain.UnitOfWork;
using IOU1.Persistance.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IOU1.Application.Features.Invitations.Direct.UseDirectInvitation.Handler;

public sealed class UseDirectInvitationHandler(
    IValidator<UseDirectInvitationRequest> validator,
    IUnitOfWork unit,
    IOU1Context context,
    IAuthUser user,
    ILogger<UseDirectInvitationHandler> logger,
    IMemberService memberService)
    : IUseDirectInvitationHandler
{
    private readonly IValidator<UseDirectInvitationRequest> _validator = validator;
    private readonly IUnitOfWork _unit = unit;
    private readonly IOU1Context _context = context;
    private readonly IAuthUser _user = user;
    private readonly ILogger<UseDirectInvitationHandler> _logger = logger;
    private readonly IMemberService _memberService = memberService;

    public async Task<Result<UseDirectInvitationResponse>> Handle(UseDirectInvitationRequest request, CancellationToken cancellationToken = default)
    {
        var validationResult = _validator.Validate(request);
        if (!validationResult.IsValid)
            return Result<UseDirectInvitationResponse>.Failure(validationResult.Errors);

        var invitation = await _context.Invitations
            .Where(i => i.Id == request.InvitationId)
            .FirstOrDefaultAsync(cancellationToken);

        if (invitation is null)
            return Result<UseDirectInvitationResponse>.Failure("Invitation with provided ID doesn't exist!");

        if (invitation.InvitationStatus != InvitationStatus.Pending)
            return Result<UseDirectInvitationResponse>.Failure($"Invitation has invalid status! Status: {invitation.InvitationStatus}.");

        if (_user.Id != invitation.UserId)
            return Result<UseDirectInvitationResponse>.Failure("You can't accept other user's invitation.");

        try
        {
            await _unit.BeginTransaction();

            var newMemberResult = await _memberService.AddMember(invitation.GroupId, invitation.UserId, cancellationToken);
            if (!newMemberResult.IsSuccess)
                return Result<UseDirectInvitationResponse>.Failure(
                    newMemberResult.ErrorMessage ?? "Something went wrong while adding a member to the group! Try again later.");

            invitation.MarkAsAccepted();
            _context.Invitations.Update(invitation);

            await _unit.SaveChanges(cancellationToken);
            await _unit.CommitTransaction();

            return Result<UseDirectInvitationResponse>.Success(new());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Something went wrong while accepting an invitation!");
            await _unit.RollbackTransaction();

            return Result<UseDirectInvitationResponse>.Failure("Something went wrong while accepting an invitation!");
        }
    }
}

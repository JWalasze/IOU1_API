using FluentValidation;
using IOU1.Application.Features.Invitations.DirectInvitation.Models;
using IOU1.Application.Services.Invitations;
using IOU1.Domain.Exceptions;
using IOU1.Domain.Models.Results;

namespace IOU1.Application.Features.Invitations.DirectInvitation;

public sealed class DirectInvitationCreationHandler(
    IValidator<DirectInvitationCreationRequest> validator,
    IDirectInvitationCreationService service)
    : IDirectInvitationCreationHandler
{
    private readonly IValidator<DirectInvitationCreationRequest> _validator = validator;
    private readonly IDirectInvitationCreationService _service = service;

    public async Task<Result<DirectInvitationCreationResponse?>> Handle(DirectInvitationCreationRequest request, CancellationToken cancellationToken = default)
    {
        var validationResult = _validator.Validate(request);
        if (!validationResult.IsValid)
        {
            return Result<DirectInvitationCreationResponse?>.Failure(
                validationResult.Errors.Select(e => new ProblemDetails(e.ErrorMessage, e.ErrorCode)));
        }

        try
        {
            var result = await _service.MakeInvitation(request.email, request.GroupId, request.SenderId);
            if (result is null)
            {
                return Result<DirectInvitationCreationResponse?>.Failure("Failed to create invitation!");
            }

            return Result<DirectInvitationCreationResponse?>.Success(new DirectInvitationCreationResponse());
        }
        catch (UserNotFoundException)
        {
            return Result<DirectInvitationCreationResponse?>.Failure("Could not find user with this email!");
        }
        catch (Exception)
        {
            return Result<DirectInvitationCreationResponse?>.Failure("An unexpected error occured!");
        }
    }
}

using FluentValidation;
using IOU1.Application.Features.Invitations.DirectInvitation.Models;
using IOU1.Application.Mediator;
using IOU1.Application.Services.Invitations;
using IOU1.Domain.Entities;
using IOU1.Domain.Exceptions;
using IOU1.Domain.Interfaces;
using IOU1.Domain.Models.Results;
using MapsterMapper;

namespace IOU1.Application.Features.Invitations.DirectInvitation;

public class DirectInvitationCreationHandler(
    IValidator<DirectInvitationCreationRequest> validator,
    IMapper mapper,
    IDirectInvitationCreationService service)
    : RequestHandler<DirectInvitationCreationRequest, DirectInvitationCreationResponse>(validator, mapper)
{
    private readonly IDirectInvitationCreationService _service = service;

    protected override async Task<IResult> Do(DirectInvitationCreationRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _service.MakeInvitation(request.email, request.GroupId, request.SenderId);
            if (result == null)
            {
                return Result<Invitation>.Failure("Failed to create invitation!");
            }

            return Result<Invitation>.Success(result);
        }
        catch (UserNotFoundException)
        {
            return Result<Invitation>.Failure("Could not find user with this email!");
        }
        catch (Exception)
        {
            return Result<Invitation>.Failure("An unexpected error occured!");
        }
    }
}

using FluentValidation;
using IOU1.Application.Features.Invitations.DirectInvitation.Models;
using IOU1.Application.Mediator;
using IOU1.Application.Service;
using IOU1.Domain.Exceptions;

namespace IOU1.Application.Features.Invitations.DirectInvitation;

public class DirectInvitationCreationHandler(IValidator<DirectInvitationCreationRequest> validator, IDirectInvitationCreationService service) : IRequestHandler<DirectInvitationCreationRequest, DirectInvitationCreationResponse>
{
    private readonly IValidator<DirectInvitationCreationRequest> _validator = validator;
    private readonly IDirectInvitationCreationService _service = service;

    public async Task<DirectInvitationCreationResponse> Handle(DirectInvitationCreationRequest request, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return new()
            {
                ErrorMessage = validationResult.Errors.First().ErrorMessage,
            };
        }

        try
        {
            var result = await _service.MakeInvitation(request.email, request.GroupId, request.SenderId);
            if (result == null)
            {
                return new() { ErrorMessage = "Failed to create invitation!" };
            }
        }
        catch (UserNotFoundException e)
        {
            {
                return new() { ErrorMessage = "Could not find user with this email!" };
            }
        }
        return new();
    }
}

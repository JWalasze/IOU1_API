using FluentValidation;
using FluentValidation.Results;
using IOU1.Application.Features.Invitations.UseInvitationLink.Models;
using IOU1.Application.Mediator;
using IOU1.Application.Service;
using IOU1.Domain.Interfaces;
using IOU1.Domain.Models;

namespace IOU1.Application.Features.Invitations.UseInvitationLink;

public class UseInvitationLinkHandler(IInvitationLinkService generateInvitationService, IValidator<UseInvitationLinkRequest> validator) : RequestHandler<UseInvitationLinkRequest, UseInvitationLinkResponse>(validator)
{
    private readonly IInvitationLinkService _generateInvitationService = generateInvitationService;

    protected override async Task<IResult> Do(UseInvitationLinkRequest request, CancellationToken cancellationToken = default)
    {
        //Add logic here
        await _generateInvitationService.UseDirectInvitation((long)request.InvitationId);

        return Result.Success();
    }

    protected override UseInvitationLinkResponse MapFailure(IResult? result)
    {
        return new UseInvitationLinkResponse()
        {
            IsSuccess = false,
            ErrorMessage = result?.ErrorMessage
        };
    }

    protected override UseInvitationLinkResponse MapFailureValidationResult(ValidationResult result)
    {
        return new UseInvitationLinkResponse()
        {
            ErrorMessage = result.Errors.FirstOrDefault()?.ErrorMessage,
            IsSuccess = false
        };
    }

    //If we have a mapper as class...
    protected override UseInvitationLinkResponse MapSuccess(IResult result)
    {
        return new UseInvitationLinkResponse();
    }
}

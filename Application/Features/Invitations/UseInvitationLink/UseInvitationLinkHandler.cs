using FluentValidation;
using FluentValidation.Results;
using IOU1.Application.Features.Invitations.UseInvitationLink.Models;
using IOU1.Application.Mediator;
using IOU1.Application.Service;
using IOU1.Domain.Interfaces;
using IOU1.Domain.Models;
using MapsterMapper;

namespace IOU1.Application.Features.Invitations.UseInvitationLink;

public class UseInvitationLinkHandler(IValidator<UseInvitationLinkRequest> validator, IMapper mapper, IInvitationLinkService generateInvitationService) : RequestHandler<UseInvitationLinkRequest, UseInvitationLinkResponse>(validator, mapper)
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
            ErrorMessage = result?.ErrorMessage
        };
    }

    protected override UseInvitationLinkResponse MapFailure(ValidationResult result)
    {
        return new UseInvitationLinkResponse()
        {
            ErrorMessage = result.Errors.FirstOrDefault()?.ErrorMessage,
        };
    }

    //If we have a mapper as class...
    protected override UseInvitationLinkResponse MapSuccess(IResult result)
    {
        return new UseInvitationLinkResponse();
    }
}

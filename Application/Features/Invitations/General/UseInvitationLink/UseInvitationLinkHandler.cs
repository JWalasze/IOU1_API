using FluentValidation;
using IOU1.Application.Features.Invitations.General.UseInvitationLink.Models;
using IOU1.Application.Services.Invitations.General;
using IOU1.Domain.Models.Results;

namespace IOU1.Application.Features.Invitations.General.UseInvitationLink;

public sealed class UseInvitationLinkHandler(
    IValidator<UseInvitationLinkRequest> validator,
    IInvitationLinkService generateInvitationService)
    : IUseInvitationLinkHandler
{
    private readonly IValidator<UseInvitationLinkRequest> _validator = validator;
    private readonly IInvitationLinkService _generateInvitationService = generateInvitationService;

    public async Task<Result<UseInvitationLinkResponse?>> Handle(UseInvitationLinkRequest request, CancellationToken cancellationToken = default)
    {
        var validationResult = _validator.Validate(request);
        if (!validationResult.IsValid)
        {
            return Result<UseInvitationLinkResponse?>.Failure(
                validationResult.Errors.Select(e => new ProblemDetails(e.ErrorMessage, e.ErrorCode)));
        }

        //Add logic here
        await _generateInvitationService.UseDirectInvitation((int)request.InvitationId!, cancellationToken);

        return Result<UseInvitationLinkResponse?>.Success(new UseInvitationLinkResponse());
    }
}

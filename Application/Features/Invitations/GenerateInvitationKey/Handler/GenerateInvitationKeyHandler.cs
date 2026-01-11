using FluentValidation;
using IOU1.Application.Features.Invitations.GenerateInvitationKey.Request;
using IOU1.Application.Features.Invitations.UseInvitationLink.Models;
using IOU1.Application.Mediator;
using IOU1.Application.Services.Invitations;
using IOU1.Domain.Entities;
using IOU1.Domain.Interfaces;
using IOU1.Domain.Models;
using MapsterMapper;

namespace IOU1.Application.Features.Invitations.GenerateInvitationKey.Handler;

public class GenerateInvitationKeyHandler(
    IInvitationLinkService generateInvitationService,
    IValidator<GenerateInvitationKeyRequest> validator,
    IMapper mapper)
    : RequestHandler<GenerateInvitationKeyRequest, UseInvitationLinkResponse>(validator, mapper)
{
    private readonly IInvitationLinkService _generateInvitationService = generateInvitationService;

    protected override async Task<IResult> Do(GenerateInvitationKeyRequest request, CancellationToken cancellationToken = default)
    {
        var createdInvitationLink = await _generateInvitationService.For(request.GroupId, cancellationToken);
        if (createdInvitationLink is null)
        {
            return Result<InvitationLink?>.Failure("Invitation link couldn't be created.");
        }

        return Result<InvitationLink?>.Success(createdInvitationLink);
    }
}

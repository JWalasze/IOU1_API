using FluentValidation;
using IOU1.Application.Features.Invitations.GenerateInvitationKey.Request;
using IOU1.Application.Features.Invitations.UseInvitationLink.Models;
using IOU1.Application.Services.Invitations;
using IOU1.Domain.Models.Results;

namespace IOU1.Application.Features.Invitations.GenerateInvitationKey.Handler;

public sealed class GenerateInvitationKeyHandler(
    IInvitationLinkService generateInvitationService,
    IValidator<GenerateInvitationKeyRequest> validator)
    : IGenerateInvitationKeyHandler
{
    private readonly IInvitationLinkService _generateInvitationService = generateInvitationService;
    private readonly IValidator<GenerateInvitationKeyRequest> _validator = validator;

    public async Task<Result<UseInvitationLinkResponse?>> Handle(GenerateInvitationKeyRequest request, CancellationToken cancellationToken = default)
    {
        var validationResult = _validator.Validate(request);
        if (!validationResult.IsValid)
        {
            return Result<UseInvitationLinkResponse?>.Failure(
                validationResult.Errors.Select(e => new ProblemDetails(e.ErrorMessage, e.ErrorCode)));
        }

        var createdInvitationLink = await _generateInvitationService.For(request.GroupId, cancellationToken);
        if (createdInvitationLink is null)
        {
            return Result<UseInvitationLinkResponse?>.Failure("Invitation link couldn't be created.");
        }

        return Result<UseInvitationLinkResponse?>.Success(new UseInvitationLinkResponse
        {
            HashedKey = createdInvitationLink.InvitationKey.Key,
            ExpirationDate = createdInvitationLink.ExpirationDate.ExpirationDate
        });
    }
}

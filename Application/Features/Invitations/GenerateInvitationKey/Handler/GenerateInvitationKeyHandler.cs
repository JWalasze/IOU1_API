using FluentValidation;
using IOU1.Application.Features.Invitations.GenerateInvitationKey.Request;
using IOU1.Application.Features.Invitations.UseInvitationLink.Models;
using IOU1.Application.Mediator;
using IOU1.Application.Service;

namespace IOU1.Application.Features.Invitations.GenerateInvitationKey.Handler;

public class GenerateInvitationKeyHandler(IInvitationLinkService generateInvitationService, IValidator<GenerateInvitationKeyRequest> validator) : IRequestHandler<GenerateInvitationKeyRequest, UseInvitationLinkResponse>
{
    private readonly IValidator<GenerateInvitationKeyRequest> _validator = validator;
    private readonly IInvitationLinkService _generateInvitationService = generateInvitationService;

    public async Task<UseInvitationLinkResponse> Handle(GenerateInvitationKeyRequest request, CancellationToken cancellationToken = default)
    {
        //1 validation (bussiness rules)
        var validationResult = _validator.Validate(request);
        if (!validationResult.IsValid)
        {
            //Here hide logic in some method (IRequestHandler)
            return new UseInvitationLinkResponse
            {
                ErrorMessage = validationResult.Errors.FirstOrDefault()?.ErrorMessage
            };
        }

        //2 bussiness logic/operations
        var createdInvitationLink = await _generateInvitationService.For(request.GroupId, cancellationToken);
        if (createdInvitationLink is null)
        {
            return new UseInvitationLinkResponse
            {
                ErrorMessage = "Invitation link couldn't be created."
            };
        }

        //3 return result
        return new UseInvitationLinkResponse
        {
            HashedKey = createdInvitationLink.InvitationKey.Key,
            ExpirationDate = createdInvitationLink.ExpirationDate.ExpirationDate,
            ErrorMessage = string.Empty,
        };
    }
}

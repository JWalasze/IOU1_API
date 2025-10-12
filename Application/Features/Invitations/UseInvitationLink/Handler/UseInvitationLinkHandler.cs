using Application.Mediator;
using FluentValidation;
using IOU1.Application.Features.Invitations.UseInvitationLink.Request;
using IOU1.Application.Features.Invitations.UseInvitationLink.Response;
using IOU1.Application.Service;

namespace IOU1.Application.Features.Invitations.UseInvitationLink.Handler;

public class UseInvitationLinkHandler(IInvitationLinkService generateInvitationService, IValidator<UseInvitationLinkRequest> validator) : IRequestHandler<UseInvitationLinkRequest, UseInvitationLinkResponse>
{
    private readonly IValidator<UseInvitationLinkRequest> _validator = validator;
    private readonly IInvitationLinkService _generateInvitationService = generateInvitationService;

    public async Task<UseInvitationLinkResponse> Handle(UseInvitationLinkRequest request, CancellationToken cancellationToken = default)
    {
        //1 validation (bussiness rules)
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            //Here hide logic in some method (IRequestHandler)
            return new UseInvitationLinkResponse
            {
                IsSuccess = false,
                ErrorMessage = validationResult.Errors.FirstOrDefault()?.ErrorMessage
            };
        }

        //2 bussiness logic/operations
        var createdInvitationLink = await _generateInvitationService.For(0, cancellationToken);
        if (createdInvitationLink is null)
        {
            return new UseInvitationLinkResponse
            {
                IsSuccess = false,
                ErrorMessage = "Invitation link couldn't be created."
            };
        }

        //3 return result
        return new UseInvitationLinkResponse
        {
            HashedKey = createdInvitationLink.InvitationKey.Key,
            ExpirationDate = createdInvitationLink.ExpirationDate.ExpirationDate,
            ErrorMessage = string.Empty,
            IsSuccess = true
        };
    }
}

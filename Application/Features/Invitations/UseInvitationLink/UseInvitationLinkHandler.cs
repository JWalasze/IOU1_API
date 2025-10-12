using Application.Mediator;
using FluentValidation;
using FluentValidation.Results;
using IOU1.Application.Features.Invitations.UseInvitationLink.Models;
using IOU1.Application.Mediator;
using IOU1.Application.Service;
using IOU1.Domain.Interfaces;

namespace IOU1.Application.Features.Invitations.UseInvitationLink;

public class UseInvitationLinkHandler(IInvitationLinkService generateInvitationService, IValidator<UseInvitationLinkRequest> validator) : RequestHandler<UseInvitationLinkRequest, UseInvitationLinkResponse>
{
    private readonly IValidator<UseInvitationLinkRequest> _validator = validator;
    private readonly IInvitationLinkService _generateInvitationService = generateInvitationService;

    //public async Task<UseInvitationLinkResponse> Handle(UseInvitationLinkRequest request, CancellationToken cancellationToken = default)
    //{
    //    //1 validation (bussiness rules)
    //    var validationResult = await _validator.ValidateAsync(request, cancellationToken);
    //    if (!validationResult.IsValid)
    //    {
    //        //Here hide logic in some method (IRequestHandler)
    //        return new UseInvitationLinkResponse
    //        {
    //            IsSuccess = false,
    //            ErrorMessage = validationResult.Errors.FirstOrDefault()?.ErrorMessage
    //        };
    //    }

    //    //2 bussiness logic/operations
    //    var createdInvitationLink = await _generateInvitationService.For(0, cancellationToken);
    //    if (createdInvitationLink is null)
    //    {
    //        return new UseInvitationLinkResponse
    //        {
    //            IsSuccess = false,
    //            ErrorMessage = "Invitation link couldn't be created."
    //        };
    //    }

    //    //3 return result
    //    return new UseInvitationLinkResponse
    //    {
    //        HashedKey = createdInvitationLink.InvitationKey.Key,
    //        ExpirationDate = createdInvitationLink.ExpirationDate.ExpirationDate,
    //        ErrorMessage = string.Empty,
    //        IsSuccess = true
    //    };
    //}

    protected override Task<IResult> Do(IRequest request)
    {
        throw new NotImplementedException();
    }

    protected override UseInvitationLinkResponse MapFailure(IResult? result)
    {
        throw new NotImplementedException();
    }

    protected override UseInvitationLinkResponse MapFailureValidationResult(ValidationResult result)
    {
        throw new NotImplementedException();
    }

    protected override UseInvitationLinkResponse MapSuccess(IResult result)
    {
        throw new NotImplementedException();
    }

    protected override Task<ValidationResult> Validate(UseInvitationLinkRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}

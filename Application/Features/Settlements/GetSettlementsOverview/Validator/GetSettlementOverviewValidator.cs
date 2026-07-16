using FluentValidation;
using IOU1.Application.Features.Settlements.GetSettlementsOverview.Models;

namespace IOU1.Application.Features.Settlements.GetSettlementsOverview.Validator;

public sealed class GetSettlementOverviewValidator : AbstractValidator<GetSettlementOverviewRequest>
{
    public GetSettlementOverviewValidator()
    {
        RuleFor(gso => gso.GroupId)
            .NotEmpty()
            .WithErrorCode("INVALID_GROUP_ID_ERROR")
            .WithMessage("Group ID is missing.");
    }
}

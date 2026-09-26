using FluentValidation;
using IOU1.Application.Features.Groups.GetGroupSummary.Models.Request;

namespace IOU1.Application.Features.Groups.GetGroupSummary.Validator;

public class GetGroupSummaryValidator : AbstractValidator<GetGroupSummaryRequest>
{
    public GetGroupSummaryValidator()
    {
        RuleFor(g => g.GroupId)
            .GreaterThan(0)
            .WithErrorCode("INVALID_GROUP_ID_ERROR")
            .WithMessage("GroupId must be greater than zero.");
    }
}

using FluentValidation;
using IOU1.Application.Features.Groups.GetGroup.Models.Request;

namespace IOU1.Application.Features.Groups.GetGroup.Validator;

public class GetGroupValidator : AbstractValidator<GetGroupRequest>
{
    public GetGroupValidator()
    {
        RuleFor(g => g.GroupId)
            .GreaterThan(0)
            .WithErrorCode("INVALID_GROUP_ID_ERROR")
            .WithMessage("GroupId must be greater than zero.");
    }
}

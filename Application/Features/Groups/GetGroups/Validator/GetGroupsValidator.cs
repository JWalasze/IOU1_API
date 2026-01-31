using FluentValidation;
using IOU1.Application.Features.Groups.GetGroups.Models.Request;

namespace IOU1.Application.Features.Groups.GetGroups.Validator;

public class GetGroupsValidator : AbstractValidator<GetGroupsRequest>
{
    public GetGroupsValidator()
    {
        //RuleFor(g => g.Status).NotEmpty().WithMessage("Status cannot be empty.");
    }
}

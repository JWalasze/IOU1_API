using Application.Features.Groups.GetGroups.Request;
using FluentValidation;

namespace IOU1.Application.Features.Groups.GetGroups.Validator;

public class GetGroupsValidator : AbstractValidator<GroupsRequest>
{
    public GetGroupsValidator()
    {
        RuleFor(g => g.Status).NotEmpty().WithMessage("Status cannot be empty.");
    }
}

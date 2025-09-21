using Application.Features.Groups.Request;
using FluentValidation;

namespace Application.Features.Groups.Validator;

public class GetGroupsValidator : AbstractValidator<GroupsRequest>
{
    public GetGroupsValidator()
    {
        RuleFor(g => g.Status).NotEmpty().WithMessage("Status cannot be empty.");
    }
}

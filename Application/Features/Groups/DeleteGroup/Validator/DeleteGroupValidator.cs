using Application.Features.Groups.DeleteGroup.Request;
using FluentValidation;

namespace Application.Features.Groups.DeleteGroup.Validator;

public class DeleteGroupValidator : AbstractValidator<DeleteGroupRequest>
{
    public DeleteGroupValidator()
    {
        RuleFor(dg => dg.GroupId).GreaterThan(0).WithMessage("GroupId must be greater than 0.");
    }
}

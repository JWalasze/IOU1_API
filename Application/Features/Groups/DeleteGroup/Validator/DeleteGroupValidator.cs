using FluentValidation;
using IOU1.Application.Features.Groups.DeleteGroup.Request;

namespace Application.Features.Groups.DeleteGroup.Validator;

public class DeleteGroupValidator : AbstractValidator<DeleteGroupRequest>
{
    public DeleteGroupValidator()
    {
        RuleFor(dg => dg.GroupId).GreaterThan(0).WithMessage("GroupId must be greater than 0.");
    }
}

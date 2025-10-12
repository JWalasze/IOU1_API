using Application.Features.Groups.AddGroup.Request;
using FluentValidation;

namespace IOU1.Application.Features.Groups.AddGroup.Validator;

public class AddGroupValidator : AbstractValidator<AddGroupRequest>
{
    public AddGroupValidator()
    {

    }
}

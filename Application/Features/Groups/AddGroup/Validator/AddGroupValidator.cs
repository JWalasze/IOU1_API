using Application.Features.Groups.AddGroup.Request;
using FluentValidation;

namespace Application.Features.Groups.AddGroup.Validator;

public class AddGroupValidator : AbstractValidator<AddGroupRequest>
{
    public AddGroupValidator()
    {

    }
}

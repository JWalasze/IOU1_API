using Application.Features.AddGroup.Request;
using FluentValidation;

namespace Application.Features.AddGroup.Validator;

public class AddGroupValidator : AbstractValidator<AddGroupRequest>
{
    public AddGroupValidator()
    {

    }
}

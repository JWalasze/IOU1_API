using Application.Features.AddGroup.Request;
using Application.Features.AddGroup.Response;
using Application.Mediator;
using FluentValidation;

namespace Application.Features.AddGroup.Handler;

public class AddGroupHandler(IValidator<AddGroupRequest> validator) : IRequestHandler<AddGroupRequest, AddGroupResponse>
{
    private readonly IValidator<AddGroupRequest> _validator = validator;

    public async Task<AddGroupResponse> Handle(AddGroupRequest request, CancellationToken cancellationToken)
    {
        //1 Validation
        var validationResult = _validator.Validate(request);
        if (!validationResult.IsValid)
        {

        }

        //2 Bussiness logic

        //3 Return response
        return new AddGroupResponse();
    }
}

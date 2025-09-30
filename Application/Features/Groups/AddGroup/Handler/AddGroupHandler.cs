using Application.Features.Groups.AddGroup.Request;
using Application.Features.Groups.AddGroup.Response;
using Application.Mediator;
using Application.Service;
using FluentValidation;

namespace Application.Features.Groups.AddGroup.Handler;

public class AddGroupHandler(IValidator<AddGroupRequest> validator, IGroupService groupService) : IRequestHandler<AddGroupRequest, AddGroupResponse>
{
    private readonly IValidator<AddGroupRequest> _validator = validator;
    private readonly IGroupService _groupService = groupService;

    public async Task<AddGroupResponse> Handle(AddGroupRequest request, CancellationToken cancellationToken)
    {
        //1 Validation
        var validationResult = _validator.Validate(request);
        if (!validationResult.IsValid)
        {
            return new()
            {
                IsSuccess = false,
                ErrorMessage = validationResult.Errors.First().ErrorMessage
            };
        }

        //2 Bussiness logic
        var result = await _groupService.AddGroup(request.MemberIds, request.OwnerId, request.Description, cancellationToken);

        //3 Return response
        return new AddGroupResponse();
    }
}

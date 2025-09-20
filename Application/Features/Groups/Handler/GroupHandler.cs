using Application.Features.Groups.Request;
using Application.Features.Groups.Response;
using Application.Features.Mapper;
using Application.Mediator;
using FluentValidation;

namespace Application.Features.Groups.Handler;

public class GroupHandler(IGetGroupsQuery repository, IValidator<GroupsRequest> validator) : IRequestHandler<GroupsRequest, GroupsResponse>
{
    private readonly IGetGroupsQuery _repository = repository;
    private readonly IValidator<GroupsRequest> _validator = validator;

    public async Task<GroupsResponse> Handle(GroupsRequest request, CancellationToken cancellationToken = default)
    {
        //1 validation (bussiness rules)
        var validationResult = _validator.Validate(request);
        if (!validationResult.IsValid)
        {
            //Here hide logic in some method (IRequestHandler)
            return new GroupsResponse
            {
                GroupInfoResponse = [],
                ErrorMessage = validationResult.Errors.FirstOrDefault()?.ErrorMessage
            };
        }

        //2 bussiness logic/operations
        var groupsInfo = await _repository.GetGroups(request.UserId, cancellationToken);

        //3 return result
        return new GroupsResponse
        {
            GroupInfoResponse = groupsInfo.MapToDto()
        };
    }
}

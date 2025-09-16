using Application.Features.Groups.Request;
using Application.Features.Groups.Response;
using Application.Mediator;

namespace Application.Features.Groups.Handler;

public class GroupHandler : IRequestHandler<GroupsRequest, GroupsResponse>
{
    public Task<GroupsResponse> Handle(GroupsRequest request)
    {
        //1 validation (bussiness rules)

        //2 bussiness logic/operations

        //3 return result

        return null;
    }
}

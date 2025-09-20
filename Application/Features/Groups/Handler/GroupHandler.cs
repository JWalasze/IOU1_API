using Application.Features.Groups.Request;
using Application.Features.Groups.Response;
using Application.Features.Mapper;
using Application.Mediator;
using Domain.Entities;

namespace Application.Features.Groups.Handler;

public class GroupHandler(IGetGroupsQuery repository) : IRequestHandler<GroupsRequest, GroupsResponse>
{
    private readonly IGetGroupsQuery repository = repository;

    public async Task<GroupsResponse> Handle(GroupsRequest request)
    {
        //1 validation (bussiness rules)

        //2 bussiness logic/operations

        //3 return result

        var groupsInfo = await repository.GetGroups(request.UserId);
        return new GroupsResponse
        {
            GroupInfoResponse = groupsInfo.MapToDto()
        };
    }
}

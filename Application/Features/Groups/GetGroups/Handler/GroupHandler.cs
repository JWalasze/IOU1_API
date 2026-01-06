using Application.Features.Groups.GetGroups.Dto;
using Application.Features.Groups.GetGroups.Query;
using Application.Features.Groups.GetGroups.Request;
using Application.Features.Groups.GetGroups.Response;
using FluentValidation;
using IOU1.Application.Mediator;
using IOU1.Domain.Interfaces;
using IOU1.Domain.Models;
using MapsterMapper;

namespace IOU1.Application.Features.Groups.GetGroups.Handler;

public class GroupHandler(IGetGroupsQuery repository, IValidator<GroupsRequest> validator, IMapper mapper)
    : RequestHandler<GroupsRequest, GroupsResponse>(validator, mapper)
{
    private readonly IGetGroupsQuery _repository = repository;

    protected override async Task<IResult> Do(GroupsRequest request, CancellationToken cancellationToken = default)
    {
        var groupsInfo = await _repository.GetGroups(request.UserId, cancellationToken);
        return Result<ICollection<GetGroupsDto>>.Success(groupsInfo);
    }
}

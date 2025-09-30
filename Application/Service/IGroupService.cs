using Application.Features.Groups.AddGroup.Dto;
using Application.Features.Groups.DeleteGroup.Dto;
using Domain.Models;

namespace Application.Service;

public interface IGroupService
{
    Task<Result<AddedGroupDto?>> AddGroup(IEnumerable<long> memberIds, long ownerId, string description, CancellationToken cancellationToken = default);

    Task<Result<DeleteGroupDto?>> DeleteGroup(long groupId, CancellationToken cancellationToken = default);
}

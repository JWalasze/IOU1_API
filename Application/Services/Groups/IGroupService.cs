using Application.Features.Groups.DeleteGroup.Dto;
using IOU1.Application.Features.Groups.AddGroup.Models;
using IOU1.Domain.Models;

namespace IOU1.Application.Services.Groups;

public interface IGroupService
{
    Task<Result<AddedGroup?>> AddGroup(IEnumerable<long> memberIds, long ownerId, string name, string? description, CancellationToken cancellationToken = default);

    Task<Result<DeleteGroupDto?>> DeleteGroup(long groupId, CancellationToken cancellationToken = default);
}

using Application.Features.Groups.DeleteGroup.Dto;
using IOU1.Application.Features.Groups.AddGroup.Models;
using IOU1.Domain.Models.Results;

namespace IOU1.Application.Services.Groups;

public interface IGroupService
{
    Task<Result<AddedGroup?>> AddGroup(IEnumerable<int> memberIds, int ownerId, string name, string? description, string currencyKey, CancellationToken cancellationToken = default);

    Task<Result<DeleteGroupDto?>> DeleteGroup(int groupId, CancellationToken cancellationToken = default);
}

using Application.Features.AddGroup.Dto;
using Domain.Models;

namespace Application.Features.AddGroup.Service;

public interface IGroupService
{
    Task<Result<AddedGroupDto?>> AddGroup(IEnumerable<long> memberIds, long ownerId, string description, CancellationToken cancellationToken = default);
}

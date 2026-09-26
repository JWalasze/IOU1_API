using Application.Features.Groups.DeleteGroup.Dto;
using IOU1.Application.Features.Groups.AddGroup.Models;
using IOU1.Domain.Models.Results;

namespace IOU1.Application.Services.Groups;

public interface IGroupService
{
    Task<Result<AddedGroup?>> Add(int ownerId, string name, string? description, string currencyKey, CancellationToken cancellationToken = default);
    Task<Result<DeletedGroup?>> Delete(int groupId, CancellationToken cancellationToken = default);
}

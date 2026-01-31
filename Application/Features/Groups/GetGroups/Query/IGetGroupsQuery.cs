using IOU1.Application.Features.Groups.GetGroups.Models.Dto;

namespace IOU1.Application.Features.Groups.GetGroups.Query;

public interface IGetGroupsQuery
{
    Task<ICollection<GetGroupsDto>> GetGroups(long userId, CancellationToken cancellationToken = default);
}

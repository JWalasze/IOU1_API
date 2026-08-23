using IOU1.Application.Features.Groups.GetGroups.Models.Dto;

namespace IOU1.Application.Features.Groups.GetGroups.Query;

public interface IGetGroupsQuery
{
    Task<List<GetGroupsDto>> GetGroups(int userId, CancellationToken cancellationToken = default);
}

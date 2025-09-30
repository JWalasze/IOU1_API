using Application.Features.Groups.GetGroups.Dto;

namespace Application.Features.Groups.GetGroups.Query;

public interface IGetGroupsQuery
{
    Task<ICollection<GetGroupsDto>> GetGroups(long userId, CancellationToken cancellationToken = default);
}

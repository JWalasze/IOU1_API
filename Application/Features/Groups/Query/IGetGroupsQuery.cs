using Application.Features.Groups.Dto;

namespace Application.Features.Groups.Query;

public interface IGetGroupsQuery
{
    Task<ICollection<GetGroupsDto>> GetGroups(long userId, CancellationToken cancellationToken = default);
}

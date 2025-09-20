using Application.Features.Groups.Dtos;

namespace Application.Features.Groups;

public interface IGetGroupsQuery
{
    Task<ICollection<GetGroupsDto>> GetGroups(long userId, CancellationToken cancellationToken = default);
}

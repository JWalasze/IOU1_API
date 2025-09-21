using Application.Features.AddGroup.Dto;

namespace Application.Features.AddGroup.Service;

public interface IGroupService
{
    Task<AddedGroupDto?> AddGroup(IEnumerable<long> memberIds);
}

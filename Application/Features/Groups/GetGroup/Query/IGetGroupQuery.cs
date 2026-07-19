using IOU1.Application.Features.Groups.GetGroup.Models.Dto;

namespace IOU1.Application.Features.Groups.GetGroup.Query;

public interface IGetGroupQuery
{
    Task<GetGroupDto?> GetGroup(int groupId, CancellationToken cancellationToken = default);

    Task<IEnumerable<GetGroupExpenseDto>> GetLastExpenses(int groupId, int lastExpenseCount, CancellationToken cancellationToken = default);
}

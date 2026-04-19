using IOU1.Application.Features.Groups.GetGroup.Models.Dto;

namespace IOU1.Application.Features.Groups.GetGroup.Query;

public interface IGetGroupQuery
{
    Task<GetGroupDto?> GetGroup(long groupId, CancellationToken cancellationToken = default);

    Task<IEnumerable<GetGroupExpenseDto>> GetLastExpenses(long groupId, int lastExpenseCount, CancellationToken cancellationToken = default);
}

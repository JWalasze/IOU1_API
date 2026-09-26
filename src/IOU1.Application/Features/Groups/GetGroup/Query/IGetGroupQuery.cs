using IOU1.Application.Features.Groups.GetGroup.Models.Dto;

namespace IOU1.Application.Features.Groups.GetGroup.Query;

public interface IGetGroupQuery
{
    Task<bool> IsThatYourGroup(int userId, int groupId, CancellationToken cancellationToken = default);
    Task<GroupDto?> GetGroup(int groupId, CancellationToken cancellationToken = default);
    Task<List<ExpenseDto>> GetLastExpenses(int groupId, int lastExpenseCount, CancellationToken cancellationToken = default);
    Task<List<MemberBalanceDto>> GetBalances(int groupId, CancellationToken cancellationToken = default);
}

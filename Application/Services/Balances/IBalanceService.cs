using IOU1.Domain.Entities;
using IOU1.Domain.Models.Results;

namespace IOU1.Application.Services.Balances;

public interface IBalanceService
{
    Task<Result<List<MemberBalance>>> AddInitialBalancesFor(
        GroupMember newMember,
        CancellationToken cancellationToken = default);
}

using IOU1.Domain.Entities;
using IOU1.Domain.Models.Results;
using IOU1.Persistance.Context;
using Microsoft.EntityFrameworkCore;

namespace IOU1.Application.Services.Balances;

public sealed class BalanceService(IOU1Context context) : IBalanceService
{
    private readonly IOU1Context _context = context;

    public async Task<Result<List<MemberBalance>>> AddInitialBalancesFor(
        GroupMember newMember,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var balancesInDatabase = await _context.MemberBalances
                .Where(mb =>
                    mb.MemberId == newMember.Id ||
                    mb.CounterpartyMemberId == newMember.Id)
                .Select(mb => new { mb.MemberId, mb.CounterpartyMemberId })
                .ToListAsync(cancellationToken);

            var balanceAsMemberLookup = balancesInDatabase
                .Where(b => b.MemberId == newMember.Id)
                .Select(b => b.CounterpartyMemberId)
                .ToHashSet();

            var balanceAsCounterpartyLookup = balancesInDatabase
                .Where(b => b.CounterpartyMemberId == newMember.Id)
                .Select(b => b.CounterpartyMemberId)
                .ToHashSet();

            var balances = new List<MemberBalance>();
            foreach (var member in newMember.Group.Members.Where(m => m.UserId != newMember.UserId))
            {
                if (!balanceAsMemberLookup.Contains(member.Id))
                {
                    var balance = new MemberBalance(newMember, member, 0);
                    balances.Add(balance);
                    _context.MemberBalances.Add(balance);
                }

                if (!balanceAsCounterpartyLookup.Contains(member.Id))
                {
                    var balance = new MemberBalance(member, newMember, 0);
                    balances.Add(balance);
                    _context.MemberBalances.Add(balance);
                }
            }

            return Result<List<MemberBalance>>.Success(balances);
        }
        catch (Exception ex)
        {
            return Result<List<MemberBalance>>.Failure(
                ex, $"Error while adding initial balances for a new member {newMember.Id}");
        }
    }
}

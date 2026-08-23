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
            var balancesInDatabase = await _context
                .MemberBalances
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
                .Select(b => b.MemberId)
                .ToHashSet();

            var balances = new List<MemberBalance>();
            foreach (var member in newMember.Group.Members.Where(m => m.UserId != newMember.UserId))
            {
                if (!balanceAsMemberLookup.Contains(member.Id))
                {
                    var balance = new MemberBalance(newMember, member, newMember.Group, 0);
                    balances.Add(balance);
                    _context.MemberBalances.Add(balance);
                }

                if (!balanceAsCounterpartyLookup.Contains(member.Id))
                {
                    var balance = new MemberBalance(member, newMember, newMember.Group, 0);
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

    public Task<List<MemberBalance>> GetBalancesFor(
        Group group,
        CancellationToken cancellationToken = default)
    {
        return _context
            .MemberBalances
            .Include(mb => mb.Member)
            .Include(mb => mb.CounterpartyMember)
            .Where(mb => mb.GroupId == group.Id &&
                (group.Members.Contains(mb.Member) &&
                group.Members.Contains(mb.CounterpartyMember)))
            .ToListAsync(cancellationToken);
    }

    public void AdjustBalancesForNewExpense(
        Expense expense,
        IEnumerable<MemberBalance> balances,
        IEnumerable<ExpenseShareSettlement>? expenseShareSettlements = null)
    {
        var balancesLookup = balances.ToDictionary(
            mb => (mb.MemberId, mb.CounterpartyMemberId),
            mb => mb);

        foreach (var expenseShare in expense.Shares.Where(s => s.MemberId != expense.PayerId))
        {
            var key = (expense.PayerId, expenseShare.MemberId);
            if (!balancesLookup.TryGetValue(key, out var balanceFromPayerToMember))
                throw new InvalidOperationException($"Balance not found for payer with id: {expense.PayerId} and member: {expenseShare.MemberId}");

            balanceFromPayerToMember.ShiftAmount(expenseShare.Amount);

            key = (expenseShare.MemberId, expense.PayerId);
            if (!balancesLookup.TryGetValue(key, out var balanceFromMemberToPayer))
                throw new InvalidOperationException($"Balance not found for member with id: {expenseShare.MemberId} and payer: {expense.PayerId}");

            balanceFromMemberToPayer.ShiftAmount(-1 * expenseShare.Amount);
        }

        if (expenseShareSettlements is null)
            return;

        var shareSettlementsLookup = expenseShareSettlements
            .GroupBy(ess => (ess.Settlement.FromMemberId, ess.Settlement.ToMemberId))
            .ToDictionary(k => k.Key, v => v.Sum(es => Math.Abs(es.Settlement.Amount)));

        foreach (var expenseShare in expense.Shares.Where(s => s.MemberId != expense.PayerId))
        {
            var key = (expense.PayerId, expenseShare.MemberId);
            if (!shareSettlementsLookup.TryGetValue(key, out var shiftAmountFromPayerToMember))
                continue;

            if (!balancesLookup.TryGetValue(key, out var balanceFromPayerToMember))
                throw new InvalidOperationException($"Balance not found for payer with id: {expense.PayerId} and member: {expenseShare.MemberId}");

            balanceFromPayerToMember.ShiftAmount(-1 * shiftAmountFromPayerToMember);

            key = (expenseShare.MemberId, expense.PayerId);
            if (!shareSettlementsLookup.TryGetValue(key, out var shiftAmountFromMemberToPayer))
                continue;

            if (!balancesLookup.TryGetValue(key, out var balanceFromMemberToPayer))
                throw new InvalidOperationException($"Balance not found for member with id: {expenseShare.MemberId} and payer: {expense.PayerId}");

            balanceFromMemberToPayer.ShiftAmount(shiftAmountFromPayerToMember);
        }
    }
}

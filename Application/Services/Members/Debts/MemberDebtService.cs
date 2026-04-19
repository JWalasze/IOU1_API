using IOU1.Domain.Entities;
using IOU1.Domain.Exceptions;
using IOU1.Domain.Models.Results;
using IOU1.Persistance.Context;
using Microsoft.EntityFrameworkCore;

namespace IOU1.Application.Services.Members.Debts;

public class MemberDebtService(IOU1Context context) : IMemberDebtService
{
    private readonly IOU1Context _context = context;

    public async Task<Result> UpdateMemberDebts(Expense expense, CancellationToken cancellationToken = default)
    {
        try
        {
            var transactionsOverview = expense.Transactions
                .Select(t => new
                {
                    t.BuyerMemberId,
                    t.BorrowerMemberId,
                    t.Amount
                })
                .GroupBy(t => new { t.BuyerMemberId, t.BorrowerMemberId })
                .Select(g => new
                {
                    g.Key.BuyerMemberId,
                    g.Key.BorrowerMemberId,
                    TotalAmount = g.Sum(t => t.Amount)
                })
                .ToList();

            //Przydałaby się tabelka tymczasowa, żeby nie robić wielu zapytań do bazy danych, ale to już później
            foreach (var transaction in transactionsOverview)
            {
                var memberDebt = await _context.GroupMemberDebts
                    .FirstOrDefaultAsync(d =>
                        d.MemberId == transaction.BuyerMemberId &&
                        d.DebtorId == transaction.BorrowerMemberId,
                        cancellationToken: cancellationToken);

                if (memberDebt is null)
                {
                    await HandleNewMemberDebt(
                        transaction.BuyerMemberId,
                        transaction.BorrowerMemberId,
                        expense.GroupId,
                        transaction.TotalAmount,
                        cancellationToken);
                }
                else
                {
                    await HandleExistingMemberDebt(
                        memberDebt,
                        transaction.TotalAmount);
                }

                var debtorDebt = await _context.GroupMemberDebts
                    .FirstOrDefaultAsync(d =>
                        d.MemberId == transaction.BorrowerMemberId &&
                        d.DebtorId == transaction.BuyerMemberId,
                        cancellationToken: cancellationToken);

                if (debtorDebt is null)
                {
                    await HandleNewMemberDebt(
                        transaction.BorrowerMemberId,
                        transaction.BuyerMemberId,
                        expense.GroupId,
                        -1 * transaction.TotalAmount,
                        cancellationToken);
                }
                else
                {
                    await HandleExistingMemberDebt(
                        debtorDebt,
                        -1 * transaction.TotalAmount);
                }
            }

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure($"An error occurred while updating member debts: {ex.Message}");
        }
    }

    private async Task HandleNewMemberDebt(long memberId, long debtorId, long groupId, decimal totalAmount, CancellationToken cancellationToken = default)
    {
        var buyerMember = await _context.GroupMembers.FindAsync(
                    [memberId],
                    cancellationToken)
            ?? throw new CreateGroupMemberDebt($"Group member with id {memberId} not found.");

        var borrowerMember = await _context.GroupMembers.FindAsync(
            [debtorId],
            cancellationToken)
            ?? throw new CreateGroupMemberDebt($"Group member with id {debtorId} not found.");

        var group = await _context.Groups.FindAsync(
            [groupId],
            cancellationToken)
            ?? throw new CreateGroupMemberDebt($"Group with id {groupId} not found.");

        var newMemberDebt = GroupMemberDebt.Create(
            member: buyerMember,
            debtor: borrowerMember,
            group: group,
            balance: totalAmount);

        _context.GroupMemberDebts.Add(newMemberDebt);
    }

    private async Task HandleExistingMemberDebt(GroupMemberDebt memberDebt, decimal totalAmount)
    {
        memberDebt.ShiftBalance(totalAmount);
        _context.GroupMemberDebts.Update(memberDebt);
    }
}

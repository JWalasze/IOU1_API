using IOU1.Domain.Base;

namespace IOU1.Domain.Entities;

public class GroupMemberDebt : Entity
{
    public long MemberId { get; }
    public GroupMember Member { get; } = null!;

    public long DebtorId { get; }
    public GroupMember Debtor { get; } = null!;

    public long GroupId { get; }
    public Group Group { get; } = null!;

    public decimal Balance { get; private set; }

    private GroupMemberDebt() { }

    public GroupMemberDebt(GroupMember member, GroupMember debtor, Group group, decimal balance)
    {
        Member = member;
        MemberId = member.Id;

        Debtor = debtor;
        DebtorId = debtor.Id;

        Group = group;
        GroupId = group.Id;

        Balance = balance;
    }

    public static GroupMemberDebt Create(
        GroupMember member,
        GroupMember debtor,
        Group group,
        decimal balance)
    {
        return new GroupMemberDebt(
            member,
            debtor,
            group,
            balance);
    }

    public void UpdateBalance(decimal amount)
    {
        Balance = amount;
    }

    public void ShiftBalance(decimal amount)
    {
        Balance += amount;
    }
}

using IOU1.Domain.Base;

namespace IOU1.Domain.Entities;

public sealed class MemberBalance : Entity
{
    public long MemberId { get; }
    public GroupMember Member { get; } = null!;

    public long CounterpartyMemberId { get; }
    public GroupMember CounterpartyMember { get; } = null!;

    public decimal Amount { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private MemberBalance() { }

    public MemberBalance(GroupMember member, GroupMember counterpartyMember, decimal amount)
    {
        if (member.Id == counterpartyMember.Id)
        {
            throw new InvalidOperationException("MemberBalance members must be different.");
        }

        Member = member;
        MemberId = member.Id;
        CounterpartyMember = counterpartyMember;
        CounterpartyMemberId = counterpartyMember.Id;
        Amount = amount;
        UpdatedAt = DateTime.UtcNow;
    }

    public static MemberBalance Create(GroupMember member, GroupMember counterpartyMember, decimal amount)
    {
        return new MemberBalance(member, counterpartyMember, amount);
    }

    public void UpdateAmount(decimal amount)
    {
        Amount = amount;
        UpdatedAt = DateTime.UtcNow;
    }

    public void ShiftAmount(decimal amount)
    {
        Amount += amount;
        UpdatedAt = DateTime.UtcNow;
    }
}

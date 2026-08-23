using IOU1.Domain.Base;
using IOU1.Domain.Exceptions;

namespace IOU1.Domain.Entities;

public sealed class MemberBalance : Entity
{
    public int GroupId { get; }
    public Group Group { get; } = null!;

    public int MemberId { get; }
    public GroupMember Member { get; } = null!;

    public int CounterpartyMemberId { get; }
    public GroupMember CounterpartyMember { get; } = null!;

    public decimal Amount { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private MemberBalance() { }

    public MemberBalance(GroupMember member, GroupMember counterpartyMember, Group group, decimal amount)
    {
        ArgumentNullException.ThrowIfNull(member);
        ArgumentNullException.ThrowIfNull(counterpartyMember);
        ArgumentNullException.ThrowIfNull(group);

        //Members can have 0 as Id, when they are not yet persisted to the database.
        //TODO: Think about that problem...
        if (member.User.Id == counterpartyMember.User.Id)
            throw new CreatingMemberBalanceException("Members must be different.");

        Member = member;
        MemberId = member.Id;

        CounterpartyMember = counterpartyMember;
        CounterpartyMemberId = counterpartyMember.Id;

        GroupId = group.Id;
        Group = group;

        Amount = amount;
        UpdatedAt = DateTime.UtcNow;
    }

    #region Factories
    public static MemberBalance Create(GroupMember member, GroupMember counterpartyMember, Group group, decimal amount)
        => new(member, counterpartyMember, group, amount);

    #endregion

    #region Public Methods
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
    #endregion
}

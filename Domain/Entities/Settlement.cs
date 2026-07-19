using IOU1.Domain.Base;

namespace IOU1.Domain.Entities;

public sealed class Settlement : Entity
{
    public int GroupId { get; }
    public Group Group { get; } = null!;

    public int FromMemberId { get; }
    public GroupMember FromMember { get; } = null!;

    public int ToMemberId { get; }
    public GroupMember ToMember { get; } = null!;

    public decimal Amount { get; }
    public DateTime SettledAt { get; }

    private Settlement() { }

    public Settlement(Group group, GroupMember fromMember, GroupMember toMember, decimal amount)
    {
        if (amount <= 0)
        {
            throw new InvalidOperationException("Settlement amount must be positive.");
        }

        if (fromMember.Id == toMember.Id)
        {
            throw new InvalidOperationException("Settlement members must be different.");
        }

        Group = group;
        GroupId = group.Id;
        FromMember = fromMember;
        FromMemberId = fromMember.Id;
        ToMember = toMember;
        ToMemberId = toMember.Id;
        Amount = amount;
        SettledAt = DateTime.UtcNow;
    }

    public static Settlement Create(Group group, GroupMember fromMember, GroupMember toMember, decimal amount)
    {
        return new Settlement(group, fromMember, toMember, amount);
    }
}

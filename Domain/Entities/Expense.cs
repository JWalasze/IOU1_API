using IOU1.Domain.Base;
using IOU1.Domain.Models;
using IOU1.Domain.Services.Splits;

namespace IOU1.Domain.Entities;

public sealed class Expense : Entity
{
    public string Title { get; } = null!;
    public string? Description { get; } = null!;
    public decimal Amount { get; }
    public DateTime CreatedAt { get; }
    public bool IsDeleted { get; private set; }
    public bool IsSettled { get; private set; }

    public Group Group { get; } = null!;
    public long GroupId { get; }

    public GroupMember Payer { get; } = null!;
    public long PayerId { get; }

    public ICollection<ExpenseShare> ExpenseShares { get; } = [];
    public ICollection<Split> Splits { get; } = [];

    private Expense() { }

    public Expense(
        decimal totalAmount,
        string title,
        string? description,
        Group group,
        GroupMember payer,
        IEnumerable<Split> splits,
        ISplitStrategy splitStrategy)
    {
        Amount = totalAmount;
        Title = title;
        Description = description;
        Group = group;
        Payer = payer;
        PayerId = payer.Id;
        CreatedAt = DateTime.UtcNow;

        ValidateSplits(splits);
        Splits = [.. splits];
        ExpenseShares = [.. splitStrategy.Split(this)];
    }

    private void ValidateSplits(IEnumerable<Split> splits)
    {
        foreach (var member in splits)
        {
            if (!Group.Members.Any(m => m.UserId == member.MemberId))
            {
                throw new InvalidOperationException($"Member {member.MemberId} doesn't belong to the group {Group.Id}");
            }
        }
    }

    public void Delete()
    {
        IsDeleted = true;
    }

    public void MarkAsSettled()
    {
        IsSettled = true;
    }
}

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

    public int GroupId { get; }
    public Group Group { get; } = null!;

    public int PayerId { get; }
    public GroupMember Payer { get; } = null!;

    public int CategoryId { get; }
    public ExpenseCategory Category { get; } = null!;

    public int SplitId { get; }
    public ExpenseSplit Split { get; } = null!;

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
        ISplitStrategy splitStrategy,
        int? categoryId = null,
        int? splitId = null)
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

        CategoryId = (int)categoryId!;
        SplitId = (int)splitId!;
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

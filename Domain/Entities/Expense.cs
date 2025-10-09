using Domain.Entities;
using IOU1.Domain.Base;
using IOU1.Domain.Models;

namespace IOU1.Domain.Entities;

public class Expense : Entity
{
    public decimal TotalAmount { get; }
    public string Title { get; } = null!;
    public string? Description { get; } = null!;
    public DateTime CreatedAt { get; }
    public Group Group { get; } = null!;
    public User Buyer { get; } = null!;
    public Currency Currency { get; } = null!;
    public ICollection<Transaction> Transactions { get; } = [];
    public ICollection<Split> Splits { get; } = [];

    private Expense() { }

    public Expense(
        decimal totalAmount,
        string title,
        string? description,
        Group group,
        User buyer,
        Currency currency,
        IEnumerable<Split> splits)
    {
        TotalAmount = totalAmount;
        Title = title;
        Description = description;
        Group = group;
        Buyer = buyer;
        Currency = currency;

        ValidateSplits(splits);
        Splits = splits.ToList();
    }

    private void ValidateSplits(IEnumerable<Split> splits)
    {
        foreach (var member in splits)
        {
            if (!Group.Members.Any(m => m.MemberId == member.MemberId))
            {
                throw new InvalidOperationException($"Member {member.MemberId} doesn't belong to the group {Group.Id}");
            }
        }
    }
}

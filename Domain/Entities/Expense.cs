using IOU1.Domain.Base;
using IOU1.Domain.Models;
using IOU1.Domain.Services.Splits;

namespace IOU1.Domain.Entities;

public class Expense : Entity
{
    public decimal TotalAmount { get; }
    public string Title { get; } = null!;
    public string? Description { get; } = null!;
    public DateTime CreatedAt { get; }

    public Group Group { get; } = null!;
    public long GroupId { get; }

    public User Buyer { get; } = null!;
    public long BuyerId { get; }

    public string CurrencyKey { get; } = null!;
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
        IEnumerable<Split> splits,
        ISplitStrategy splitStrategy)
    {
        TotalAmount = totalAmount;
        Title = title;
        Description = description;
        Group = group;
        Buyer = buyer;
        BuyerId = buyer.Id;
        Currency = currency;

        ValidateSplits(splits);
        Splits = [.. splits];
        Transactions = [.. splitStrategy.Split(this)];
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
}

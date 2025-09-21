using Domain.Base;

namespace Domain.Entities;

public class Expense : Entity
{
    public long Id { get; }
    public decimal TotalAmount { get; }
    public string Title { get; } = null!;
    public string? Description { get; } = null!;
    public Group Group { get; } = null!;
    public User Buyer { get; } = null!;
    public Currency Currency { get; } = null!;
    public ICollection<Transaction> Transactions { get; } = new List<Transaction>();

    private Expense() { }

    public Expense(
        decimal totalAmount,
        string title,
        string? description,
        Group group,
        User buyer,
        Currency currency)
    {
        TotalAmount = totalAmount;
        Title = title;
        Description = description;
        Group = group;
        Buyer = buyer;
        Currency = currency;
    }
}

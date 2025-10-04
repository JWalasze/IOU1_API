using Domain.Entities;
using IOU1.Domain.Base;

namespace IOU1.Domain.Entities;

public class Transaction : Entity
{
    public long Id { get; }
    public decimal Amount { get; }
    public DateTime CreatedAt { get; }
    public Expense Expense { get; } = null!;
    public Group Group { get; } = null!;
    public User? Buyer { get; }
    public User? Borrower { get; } = null!;
    public Currency Currency { get; } = null!;

    private Transaction() { }

    public Transaction(
        decimal amount,
        DateTime addDate,
        Expense expense,
        Group group,
        User buyer,
        User borrower,
        Currency currency)
    {
        CreatedAt = addDate;
        Amount = amount;
        Expense = expense;
        Group = group;
        Buyer = buyer;
        Borrower = borrower;
        Currency = currency;
    }

    public static Transaction CreateNewTransaction(
        decimal amount,
        Expense expense,
        Group group,
        User from,
        User to,
        Currency currency)
    {
        return new Transaction(
            amount,
            DateTime.UtcNow,
            expense,
            group,
            from,
            to,
            currency);
    }
}

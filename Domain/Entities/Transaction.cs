using Domain.Base;

namespace Domain.Entities;

public class Transaction : Entity
{
    public long Id { get; }
    public DateTime AddDate { get; }
    public DateTime ModificationDate { get; }
    public decimal Amount { get; }
    public Group Group { get; } = null!;
    public User Buyer { get; } = null!;
    public User Borrower { get; } = null!;
    public Currency Currency { get; } = null!;
    public TransactionStatus Status { get; } = null!;

    private Transaction() { }

    public Transaction(
        long id,
        DateTime addDate,
        DateTime modificationDate,
        decimal amount,
        Group group,
        User buyer,
        User borrower,
        Currency currency,
        TransactionStatus status)
    {
        Id = id;
        AddDate = addDate;
        ModificationDate = modificationDate;
        Amount = amount;
        Group = group;
        Buyer = buyer;
        Borrower = borrower;
        Currency = currency;
        Status = status;
    }
}

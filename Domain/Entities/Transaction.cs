using Domain.Base;
using System.Text.RegularExpressions;

namespace Domain.Entities;

public class Transaction : Entity
{
    public long Id { get; }
    public DateTime AddDate { get; }
    public DateTime? ModificationDate { get; }
    public decimal Amount { get; }
    public Expense Expense { get; } = null!;
    public Group Group { get; } = null!;
    public User Buyer { get; } = null!;
    public User Borrower { get; } = null!;
    public Currency Currency { get; } = null!;
    public TransactionStatus Status { get; } = null!;

    private Transaction() { }

    public Transaction(
        DateTime addDate,
        DateTime? modificationDate,
        decimal amount,
        Expense expense,
        Group group,
        User buyer,
        User borrower,
        Currency currency,
        TransactionStatus status)
    {
        AddDate = addDate;
        ModificationDate = modificationDate;
        Amount = amount;
        Expense = expense;
        Group = group;
        Buyer = buyer;
        Borrower = borrower;
        Currency = currency;
        Status = status;
    }

    public static Transaction CreateNewTransaction(
        decimal amount,
        Expense expense,
        Group group,
        User from,
        User to,
        Currency currency,
        TransactionStatus status)
    {
        return new Transaction(
            addDate: DateTime.UtcNow,
            modificationDate: null,
            amount,
            expense,
            group,
            from,
            to,
            currency,
            status);
    }
}

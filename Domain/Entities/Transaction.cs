using IOU1.Domain.Base;

namespace IOU1.Domain.Entities;

public class Transaction : Entity
{
    public decimal Amount { get; }
    public DateTime CreatedAt { get; }
    public Expense Expense { get; } = null!;
    public Group Group { get; } = null!;
    public User? Buyer { get; }
    public User? Borrower { get; } = null!;
    public long? BorrowerId { get; }

    public string CurrencyKey { get; } = null!;
    public Currency Currency { get; } = null!;

    public long BuyerMemberId { get; }
    public GroupMember BuyerMember { get; } = null!;

    public long BorrowerMemberId { get; }
    public GroupMember BorrowerMember { get; } = null!;

    private Transaction() { }

    public Transaction(
        decimal amount,
        DateTime addDate,
        Expense expense,
        Group group,
        User buyer,
        User borrower,
        Currency currency,
        GroupMember buyerMember,
        GroupMember borrowerMember)
    {
        CreatedAt = addDate;
        Amount = amount;
        Expense = expense;
        Group = group;
        Buyer = buyer;
        Borrower = borrower;
        Currency = currency;
        BuyerMember = buyerMember;
        BorrowerMember = borrowerMember;
    }

    public static Transaction CreateNewTransaction(
        decimal amount,
        Expense expense,
        Group group,
        User from,
        User to,
        Currency currency,
        GroupMember buyerMember,
        GroupMember borrowerMember)
    {
        return new Transaction(
            amount,
            DateTime.UtcNow,
            expense,
            group,
            from,
            to,
            currency,
            buyerMember,
            borrowerMember);
    }
}

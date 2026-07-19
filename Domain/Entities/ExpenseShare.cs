using IOU1.Domain.Base;

namespace IOU1.Domain.Entities;

public sealed class ExpenseShare : Entity
{
    public int ExpenseId { get; }
    public Expense Expense { get; } = null!;

    public int MemberId { get; }
    public GroupMember Member { get; } = null!;

    public decimal Amount { get; }

    private ExpenseShare() { }

    public ExpenseShare(Expense expense, GroupMember member, decimal amount)
    {
        Expense = expense;
        ExpenseId = expense.Id;
        Member = member;
        MemberId = member.Id;
        Amount = amount;
    }

    public static ExpenseShare Create(Expense expense, GroupMember member, decimal amount)
    {
        return new ExpenseShare(expense, member, amount);
    }
}

using IOU1.Domain.Base;
using IOU1.Domain.Exceptions;
using IOU1.Domain.Utils;

namespace IOU1.Domain.Entities;

public sealed class ExpenseShare : Entity
{
    public int ExpenseId { get; }
    public Expense Expense { get; } = null!;

    public int MemberId { get; }
    public GroupMember Member { get; } = null!;

    public decimal Amount { get; }


    private readonly List<ExpenseShareSettlement> _expensShareSettlements = [];
    public IReadOnlyCollection<ExpenseShareSettlement> ExpenseShareSettlements => _expensShareSettlements;

    #region Ctor
    private ExpenseShare() { }

    private ExpenseShare(Expense expense, GroupMember member, decimal amount)
    {
        ArgumentNullException.ThrowIfNull(expense);
        ArgumentNullException.ThrowIfNull(member);

        Expense = expense;
        ExpenseId = expense.Id;

        Member = member;
        MemberId = member.Id;

        Amount = amount;
    }

    private ExpenseShare(int expenseId, int memberId, decimal amount)
    {
        GuardIntId.ForPresence<CreatingExpenseShareException>(expenseId, "ExpenseId cannot be less or equal to 0.");
        ExpenseId = expenseId;

        GuardIntId.ForPresence<CreatingExpenseShareException>(memberId, "MemberId cannot be less or equal to 0.");
        MemberId = memberId;

        Amount = amount;
    }
    #endregion

    #region Factories
    public static ExpenseShare Create(Expense expense, GroupMember member, decimal amount) =>
        new(expense, member, amount);

    public static ExpenseShare Create(int expenseId, int memberId, decimal amount) =>
        new(expenseId, memberId, amount);
    #endregion
}

using IOU1.Domain.Entities;
using IOU1.Domain.Services.Splits;

namespace IOU1.Application.Strategy;

public class CustomSplitStrategy : ISplitStrategy
{
    public IEnumerable<Transaction> Split(Expense expense)
    {
        if (expense.Splits.Count == 0)
            throw new InvalidOperationException("Can't split an expense without defined inforamtion.");

        var buyer = expense.Group.Members.FirstOrDefault(m => m.UserId == expense.Buyer.Id)?.User
            ?? throw new InvalidOperationException($"Buyer {expense.Buyer.Id} doesn't exist.");

        var buyerMember = expense.Group.Members.First(m => m.UserId == buyer.Id)
            ?? throw new InvalidOperationException($"Buyer member with user id: {expense.Buyer.Id} doesn't exist.");

        decimal splitTotal = 0;
        foreach (var split in expense.Splits)
        {
            var absAmount = Math.Abs(split.Amount);
            var borrowerMember = expense.Group.Members.First(m => m.UserId == split.MemberId);
            var borrower = borrowerMember.User;

            var borrowerTransaction = new Transaction(
                -1 * absAmount,
                DateTime.UtcNow,
                expense,
                expense.Group,
                buyer,
                borrower,
                expense.Currency,
                buyerMember,
                borrowerMember);

            expense.Transactions.Add(borrowerTransaction);

            splitTotal += absAmount;
        }

        if (expense.TotalAmount < splitTotal)
            throw new InvalidOperationException($"Total amount from {splitTotal} is greater than totalAmount {expense.TotalAmount}");

        var buyerSplit = expense.Splits.FirstOrDefault(s => s.MemberId == expense.Buyer.Id);

        var leftDifference = expense.TotalAmount - splitTotal;
        var buyerSplitAmount = Math.Abs(buyerSplit?.Amount ?? 0) + leftDifference;

        if (!expense.Splits.Any(s => s.MemberId == buyer.Id))
        {
            var buyerTransactionNegative = new Transaction(
                -1 * Math.Abs(buyerSplitAmount),
                DateTime.UtcNow,
                expense,
                expense.Group,
                buyer,
                buyer,
                expense.Currency,
                buyerMember,
                buyerMember);

            expense.Transactions.Add(buyerTransactionNegative);
        }

        var buyerTransactionPositive = new Transaction(
            Math.Abs(buyerSplitAmount),
            DateTime.UtcNow,
            expense,
            expense.Group,
            buyer,
            buyer,
            expense.Currency,
            buyerMember,
            buyerMember);

        expense.Transactions.Add(buyerTransactionPositive);
        return expense.Transactions;
    }
}

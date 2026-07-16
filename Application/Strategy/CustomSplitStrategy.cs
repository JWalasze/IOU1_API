using IOU1.Domain.Entities;
using IOU1.Domain.Services.Splits;

namespace IOU1.Application.Strategy;

public class CustomSplitStrategy : ISplitStrategy
{
    public IEnumerable<ExpenseShare> Split(Expense expense)
    {
        if (expense.Splits.Count == 0)
            throw new InvalidOperationException("Can't split an expense without defined inforamtion.");

        var payerUserId = expense.Payer.UserId;

        var shares = new List<ExpenseShare>();
        decimal splitTotal = 0;

        foreach (var split in expense.Splits)
        {
            var absAmount = Math.Abs(split.Amount);
            var member = expense.Group.Members.First(m => m.UserId == split.MemberId);

            shares.Add(new ExpenseShare(expense, member, absAmount));
            splitTotal += absAmount;
        }

        if (expense.Amount < splitTotal)
            throw new InvalidOperationException($"Total amount from {splitTotal} is greater than totalAmount {expense.Amount}");

        var leftDifference = expense.Amount - splitTotal;
        var payerSplit = expense.Splits.FirstOrDefault(s => s.MemberId == payerUserId);

        if (payerSplit is not null)
        {
            var payerShare = shares.First(s => s.MemberId == expense.Payer.Id);
            shares.Remove(payerShare);
            shares.Add(new ExpenseShare(expense, expense.Payer, payerShare.Amount + leftDifference));
        }
        else if (leftDifference > 0)
        {
            shares.Add(new ExpenseShare(expense, expense.Payer, leftDifference));
        }

        return shares;
    }
}

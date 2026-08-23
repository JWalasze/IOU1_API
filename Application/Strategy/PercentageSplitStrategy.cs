using IOU1.Domain.Entities;
using IOU1.Domain.Services.Splits;

namespace IOU1.Application.Strategy;

public sealed class PercentageSplitStrategy : ISplitStrategy
{
    public IEnumerable<ExpenseShare> Split(Expense expense)
    {
        foreach (var split in expense.Splits)
        {
            //We have validated the percentage value when creating the split, so we can safely assume that it is not null here.
            var shareAmount = Math.Round(expense.Amount * split.Percentage!.Value / 100, 2);
            if (Math.Abs(shareAmount - split.Amount) > Domain.Models.Split.MinPercentageDiff)
                throw new InvalidOperationException($"Calculated share amount {shareAmount} does not match the specified amount {split.Amount} for member {split.MemberId}.");

            var member = expense.Group.Members.First(m => m.Id == split.MemberId);
            if (expense.PayerId != split.MemberId)
                shareAmount = -1 * shareAmount;

            yield return ExpenseShare.Create(expense, member, shareAmount);
        }
    }
}

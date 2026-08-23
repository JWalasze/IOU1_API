using IOU1.Domain.Entities;
using IOU1.Domain.Services.Splits;

namespace IOU1.Application.Strategy;

public sealed class CustomSplitStrategy : ISplitStrategy
{
    public IEnumerable<ExpenseShare> Split(Expense expense)
    {
        foreach (var split in expense.Splits)
        {
            //czemu pod debug wchodi tutaj 2 razy?
            var shareAmount = split.Amount;
            var member = expense.Group.Members.First(m => m.Id == split.MemberId);
            if (expense.PayerId != split.MemberId)
                shareAmount = -1 * shareAmount;

            yield return ExpenseShare.Create(expense, member, shareAmount);
        }
    }
}

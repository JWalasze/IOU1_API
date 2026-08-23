using IOU1.Domain.Entities;
using IOU1.Domain.Services.Splits;

namespace IOU1.Application.Strategy;

public sealed class EqualSplitStrategy : ISplitStrategy
{
    public IEnumerable<ExpenseShare> Split(Expense expense)
    {
        var numberOfMembers = expense.Group.Members.Count;
        var equalAmount = Math.Round(expense.Amount / numberOfMembers, 2);

        foreach (var member in expense.Group.Members)
        {
            var shareAmount = equalAmount;
            if (expense.PayerId != member.Id)
                shareAmount = -1 * shareAmount;

            yield return ExpenseShare.Create(expense, member, shareAmount);
        }
    }
}

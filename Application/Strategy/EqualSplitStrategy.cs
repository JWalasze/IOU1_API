using IOU1.Domain.Entities;
using IOU1.Domain.Services.Splits;

namespace IOU1.Application.Strategy;

public class EqualSplitStrategy : ISplitStrategy
{
    public IEnumerable<ExpenseShare> Split(Expense expense)
    {
        var numberOfMembers = expense.Group.Members.Count;
        if (numberOfMembers is 0)
        {
            throw new InvalidOperationException($"Group {expense.Group.Id} has 0 members.");
        }

        var equalAmount = expense.Amount / numberOfMembers;

        return [.. expense.Group.Members.Select(member => new ExpenseShare(expense, member, equalAmount))];
    }
}

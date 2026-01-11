using IOU1.Domain.Entities;

namespace IOU1.Application.Strategy;

public class EqualSplitStrategy(Expense expense) : BaseSplitStrategy(expense)
{
    public override void Split()
    {
        var numberOfMembers = _expense.Group.Members.Count;
        if (numberOfMembers is 0)
        {
            throw new InvalidOperationException($"Group {_expense.Group.Id} has 0  members.");
        }

        var equalAmount = _expense.TotalAmount / numberOfMembers;

        //TODO Dates to expense
        var buyerTransaction = new Transaction(equalAmount, DateTime.Now, _expense, _expense.Group, _expense.Buyer, _expense.Buyer, _expense.Currency);
        _expense.Transactions.Add(buyerTransaction);

        foreach (var member in _expense.Group.Members.Where(m => m.MemberId != _expense.Buyer.Id))
        {
            var borrowerTransaction = new Transaction(-equalAmount, DateTime.Now, _expense, _expense.Group, _expense.Buyer, member.User, _expense.Currency);
            _expense.Transactions.Add(borrowerTransaction);
        }
    }
}

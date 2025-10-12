using IOU1.Domain.Entities;

namespace IOU1.Application.Strategy;

public class CustomSplitStrategy(Expense expense) : BaseSplitStrategy(expense)
{
    public override void Split()
    {
        if (_expense.Splits.Count == 0)
            throw new InvalidOperationException("Can't split an expense without defined inforamtion.");

        var buyer = _expense.Group.Members.FirstOrDefault(m => m.MemberId == _expense.Buyer.Id)?.User
            ?? throw new InvalidOperationException($"Buyer {_expense.Buyer.Id} doesn't exist.");

        decimal splitTotal = 0;
        foreach (var split in _expense.Splits)
        {
            var absAmount = Math.Abs(split.Amount);
            var borrower = _expense.Group.Members.First(m => m.MemberId == split.MemberId).User;
            var borrowerTransaction = new Transaction(-1 * absAmount, DateTime.UtcNow, _expense, _expense.Group, buyer, borrower, _expense.Currency);

            _expense.Transactions.Add(borrowerTransaction);

            splitTotal += absAmount;
        }

        if (_expense.TotalAmount < splitTotal)
            throw new InvalidOperationException($"Total amount from {splitTotal} is greater than totalAmount {_expense.TotalAmount}");
        
        var buyerSplit = _expense.Splits.FirstOrDefault(s => s.MemberId == _expense.Buyer.Id);

        var leftDifference = _expense.TotalAmount - splitTotal;
        var buyerSplitAmount = Math.Abs(buyerSplit?.Amount ?? 0) + leftDifference;

        if (!_expense.Splits.Any(s => s.MemberId == buyer.Id))
        {
            var buyerTransactionNegative = new Transaction(-1 * Math.Abs(buyerSplitAmount), DateTime.UtcNow, _expense, _expense.Group, buyer, buyer, _expense.Currency);
            _expense.Transactions.Add(buyerTransactionNegative);
        }
        
        var buyerTransactionPositive = new Transaction(Math.Abs(buyerSplitAmount), DateTime.UtcNow, _expense, _expense.Group, buyer, buyer, _expense.Currency);
        _expense.Transactions.Add(buyerTransactionPositive);
    }
}

using IOU1.Domain.Entities;
using IOU1.Domain.Services.Splits;

namespace IOU1.Application.Strategy;

public class EqualSplitStrategy : ISplitStrategy
{
    public IEnumerable<Transaction> Split(Expense expense)
    {
        var numberOfMembers = expense.Group.Members.Count;
        if (numberOfMembers is 0)
        {
            throw new InvalidOperationException($"Group {expense.Group.Id} has 0 members.");
        }

        var equalAmount = expense.TotalAmount / numberOfMembers;
        var buyerMember = expense.Group.Members.First(m => m.UserId == expense.BuyerId);

        var buyerTransaction = new Transaction(
            amount: equalAmount,
            addDate: DateTime.Now,
            expense: expense,
            group: expense.Group,
            buyer: expense.Buyer,
            borrower: expense.Buyer,
            currency: expense.Currency,
            buyerMember: buyerMember,
            borrowerMember: buyerMember);

        expense.Transactions.Add(buyerTransaction);

        foreach (var member in expense.Group.Members)
        {
            var borrowerTransaction = new Transaction(
                amount: -1 * equalAmount,
                addDate: DateTime.Now,
                expense: expense,
                group: expense.Group,
                buyer: expense.Buyer,
                borrower: member.User,
                currency: expense.Currency,
                buyerMember: buyerMember,
                borrowerMember: member);

            expense.Transactions.Add(borrowerTransaction);
        }

        return expense.Transactions;
    }
}

using Domain.Entities;
using Domain.RepoInterfaces;
using IOU1_API.Controllers;
using IOU1_API.DTOs;
using IOU1_API.Mappers;

namespace IOU1_API.Services;

public record TransactionData(User User, decimal Amount);
public class TransactionService
{
    private readonly IUserRepository _userRepository;
    private readonly IGroupRepository _groupRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IExpenseRepository _expenseRepository;
    private readonly ICurrencyRepository _currencyRepository;
    private readonly ITransactionStatusRepository _transactionStatusRepository;

    public TransactionService(IGroupRepository groupRepository, IUserRepository userRepository, ITransactionRepository transactionRepository, IExpenseRepository expenseRepository, ICurrencyRepository currencyRepository, ITransactionStatusRepository transactionStatusRepository)
    {
        _groupRepository = groupRepository;
        _userRepository = userRepository;
        _transactionRepository = transactionRepository;
        _expenseRepository = expenseRepository;
        _currencyRepository = currencyRepository;
        _transactionStatusRepository = transactionStatusRepository;
    }

    public async Task<List<TransactionDto>> GetTransactionByGroupIdAsync(long groupId)
    {
        var result = await _transactionRepository.GetGroupTransactionsAsync(groupId);

        if (result == null)
        {
            return new();
        }

        return result.ToDtoList();
    }

    public async Task<IEnumerable<TransactionDto>> CreateGroupTransactionsAsync(
        GroupTransactionRequest request)
    {
        var group = await _groupRepository.GetByIdAsync(request.GroupId)
            ?? throw new ArgumentException("Group not found");

        var creator = await _userRepository.GetByIdAsync(request.BuyerId)
            ?? throw new ArgumentException("Buyer not found");

        Currency currency = await _currencyRepository.GetDefaultCurrency(); // TODO: hold default in group info or take it form request
        TransactionStatus pendingStatus = await _transactionStatusRepository.GetPendingStatus(); // TODO: delete this ridiculous thing

        bool equalOverride = false;
        var memberIds = InferSplitMethod(request, group, ref equalOverride);

        var borrowers = await _userRepository.GetByIdsAsync(memberIds)
            ?? throw new ArgumentException("Borrowers not found");

        List<TransactionData> transactionSplits = new();
        CalculateBorrowersSplits(request, equalOverride, borrowers, transactionSplits);
        InferCreatorsShare(request, creator, transactionSplits);

        var expense = new Expense(request.AmountTotal, request.Title, request.Description, group, creator, currency);

        foreach (TransactionData split in transactionSplits)
        {
            var to = split.User;

            var tx = Transaction.CreateNewTransaction(
                amount: split.Amount,
                expense: expense,
                group: expense.Group,
                from: creator,
                to: to,
                currency: expense.Currency,
                status: pendingStatus
            );

            expense.Transactions.Add(tx);
        }

        await _expenseRepository.AddAsync(expense);
        await _expenseRepository.SaveChangesAsync();

        return expense.Transactions.ToDtoList();
    }

    private static IEnumerable<long> InferSplitMethod(GroupTransactionRequest request, Group group, ref bool equalOverride)
    {
        IEnumerable<long> memberIds;
        if (request.Splits != null && request.Splits.Any()) //there are explicit splits
        {
            memberIds = request.Splits.Select(m => m.MemberId);
        }
        else if (request.MemberIds != null && request.MemberIds.Any()) //there are just members without amounts
        {
            memberIds = request.MemberIds;
            equalOverride = true;
        }
        else //everyone shares
        {
            memberIds = group.Members.Select(m => m.Id);
            equalOverride = true;
        }

        return memberIds;
    }

    private static void CalculateBorrowersSplits(GroupTransactionRequest request, bool equalOverride, IEnumerable<User> borrowers, List<TransactionData> transactionSplits)
    {
        // create negative transaction (borrowing, payment request)
        if (request.DivideEqually || equalOverride)
        {
            var share = request.AmountTotal / borrowers.Count();
            foreach (var member in borrowers)
            {
                transactionSplits.Add(new TransactionData(member, -share));
            }
        }
        else if (request.Splits != null && request.Splits.Any())
        {
            foreach (var member in borrowers)
            {
                var amount = request.Splits
                    .FirstOrDefault(m => m.MemberId == member.Id)
                    ?.Amount ?? 0;
                transactionSplits.Add(new TransactionData(member, -amount));
            }
        }
        else
        {
            throw new ArgumentException("Incompatible request");
        }
    }

    private static void InferCreatorsShare(GroupTransactionRequest request, User creator, List<TransactionData> transactionSplits)
    {
        var sum = -transactionSplits.Select(s => s.Amount)
                    .Sum();

        // if crator payed for himself
        var index = transactionSplits.FindIndex(a => a.User.Id == creator.Id);
        decimal difference = request.AmountTotal - sum;
        if (index >= 0)
        {
            var selfShare = transactionSplits[index];

            if (difference > 0)
            {
                // replace with corrected share
                transactionSplits[index] = selfShare with { Amount = selfShare.Amount - difference };
            }

            // add compensating transaction
            transactionSplits.Add(new TransactionData(creator, -transactionSplits[index].Amount));
        }
        else
        {
            if (difference > 0)
            {

                transactionSplits.Add(new(creator, difference));
                transactionSplits.Add(new(creator, -difference));
            }
        }
    }
}

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
    private readonly ICurrencyRepository _currencyRepository;
    private readonly ITransactionStatusRepository _transactionStatusRepository;

    public TransactionService(IGroupRepository groupRepository, IUserRepository userRepository, ITransactionRepository transactionRepository, ICurrencyRepository currencyRepository, ITransactionStatusRepository transactionStatusRepository)
    {
        _groupRepository = groupRepository;
        _userRepository = userRepository;
        _transactionRepository = transactionRepository;
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

        IEnumerable<long> memberIds;
        bool equalOverride = false;

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

        var borrowers = await _userRepository.GetByIdsAsync(memberIds)
            ?? throw new ArgumentException("Borrowers not found");

        List<TransactionData> transactionSplits = new();

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
            if(difference > 0)
            {
                
                transactionSplits.Add(new(creator, difference));
                transactionSplits.Add(new(creator, -difference));
            }
            
        }

        return await CreateTransactions(
            group,
            creator,
            transactionSplits
        );
    }

    private async Task<List<TransactionDto>> CreateTransactions(
        Group group,
        User creator,
        IEnumerable<TransactionData> customSplits)
    {
        var transactions = new List<Transaction>();

        Currency currency = await _currencyRepository.GetDefaultCurrency();
        TransactionStatus pendingStatus = await _transactionStatusRepository.GetPendingStatus();

        foreach (TransactionData split in customSplits)
        {
            var to = split.User;

            var tx = Transaction.CreateNewTransaction(
                amount: split.Amount,
                group: group,
                from: creator,
                to: to,
                currency: currency,
                status: pendingStatus
            );

            transactions.Add(tx);
        }

        var result = await _transactionRepository.AddRangeAsync(transactions);

        return result.ToDtoList();
    }
}

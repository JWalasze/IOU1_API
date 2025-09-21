using Domain.Entities;
using IOU1_API.DTOs;

namespace IOU1_API.Mappers;

public static class TransactionMapping
{
    public static TransactionDto ToDto(this Transaction tx)
    {
        return new TransactionDto(
            Id: tx.Id,
            Amount: tx.Amount,
            AddDate: tx.AddDate,
            GroupId: tx.Group.Id,
            GroupName: tx.Group.Description,
            BuyerId: tx.Buyer.Id,
            BuyerName: $"{tx.Buyer.FirstName}",
            BorrowerId: tx.Borrower.Id,
            BorrowerName: $"{tx.Borrower.FirstName}",
            CurrencyCode: tx.Currency.Name
        );
    }

    public static List<TransactionDto> ToDtoList(this IEnumerable<Transaction> txs)
        => [.. txs.Select(t => t.ToDto())];
}

namespace IOU1_API.DTOs;

public record TransactionDto(
    long Id,
    decimal Amount,
    DateTime AddDate,
    long GroupId,
    string GroupName,
    long BuyerId,
    string BuyerName,
    long BorrowerId,
    string BorrowerName,
    string CurrencyCode
);

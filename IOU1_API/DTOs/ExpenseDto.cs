namespace IOU1_API.DTOs;

public record ExpenseDto(
    long Id,
    decimal TotalAmount,
    string Title,
    string? Description,
    long GroupId,
    string GroupName,
    long BuyerId,
    string BuyerName,
    string CurrencyCode,
    DateTime CreatedAt,
    List<TransactionDto> Transactions
);

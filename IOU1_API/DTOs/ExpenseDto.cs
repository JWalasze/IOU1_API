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
    List<TransactionDto> Transactions
);

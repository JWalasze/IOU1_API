namespace IOU1.Application.Features.Groups.GetGroup.Models.Dto;

public sealed record GetGroupExpenseDto
{
    public required long ExpenseId { get; init; }

    public required long BuyerId { get; init; }

    public required decimal Amount { get; init; }

    public string? Currency { get; init; }

    public required ICollection<GetGroupTransactionDto> Transactions { get; init; } = [];
}

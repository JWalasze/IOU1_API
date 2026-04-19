namespace IOU1.Application.Features.Groups.GetGroup.Models.Dto;

public sealed record GetGroupTransactionDto
{
    public required long TransactionId { get; init; }

    public required long? BorrowerId { get; init; }

    public required decimal Amount { get; init; }

    public string? Currency { get; init; }
}

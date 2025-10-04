namespace IOU1.Application.Features.Transactions.AddTransaction.Dto;

public record Split
{
    public required long MemberId { get; init; }

    public required decimal Amount { get; init; }
}

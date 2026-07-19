namespace IOU1.Domain.Models;

public record Split
{
    public required int MemberId { get; init; }

    public required decimal Amount { get; init; }
}

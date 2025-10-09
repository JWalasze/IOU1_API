namespace IOU1.Domain.Models;

public record Split
{
    public required long MemberId { get; init; }

    public required decimal Amount { get; init; }
}

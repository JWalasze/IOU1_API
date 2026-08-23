namespace IOU1.Domain.Models;

public record Split
{
    public const decimal MinPercentageDiff = 0.01m;

    public required int MemberId { get; init; }
    public required decimal Amount { get; init; }
    public decimal? Percentage { get; init; }
}

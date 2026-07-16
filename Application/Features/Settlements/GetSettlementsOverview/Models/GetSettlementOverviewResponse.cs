namespace IOU1.Application.Features.Settlements.GetSettlementsOverview.Models;

public record GetSettlementOverviewResponse
{
    public long GroupId { get; init; }
    public IEnumerable<PaymentInfo> Payments { get; } = [];
}

public record PaymentInfo
{
    public long FromMemberId { get; init; }
    public required string FromMemberName { get; init; }

    public long ToMemberId { get; init; }
    public required string ToMemberName { get; init; }

    public decimal Amount { get; init; }
    public required string CurrencyKey { get; init; }
}

using IOU1.Application.Mediator;

namespace IOU1.Application.Features.Settlements.GetSettlementsOverview.Models;

public record GetSettlementOverviewRequest : IRequest
{
    public long GroupId { get; init; }
}

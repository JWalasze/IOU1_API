using IOU1.Application.Mediator;

namespace IOU1.Application.Features.Settlements.GetSettlementsOverview.Models;

public record GetSettlementOverviewRequest : IRequest
{
    public int GroupId { get; init; }
}

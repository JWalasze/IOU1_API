using IOU1.Application.Features.Settlements.GetSettlementsOverview.Models;
using IOU1.Domain.Models.Results;

namespace IOU1.Application.Features.Settlements.GetSettlementsOverview.Handler;

public interface IGetSettlementsOverviewHandler
{
    Task<Result<GetSettlementOverviewResponse>> Handle(GetSettlementOverviewRequest request, CancellationToken cancellationToken);
}

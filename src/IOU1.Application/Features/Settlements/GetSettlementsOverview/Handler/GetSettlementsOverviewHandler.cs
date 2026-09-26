using FluentValidation;
using IOU1.Application.Features.Settlements.GetSettlementsOverview.Models;
using IOU1.Application.Services.Settlements;
using IOU1.Domain.Models.Results;

namespace IOU1.Application.Features.Settlements.GetSettlementsOverview.Handler;

public class GetSettlementsOverviewHandler(
    IValidator<GetSettlementOverviewRequest> validator,
    ISettlementService settlementService) : IGetSettlementsOverviewHandler
{
    private readonly IValidator<GetSettlementOverviewRequest> _validator = validator;
    private readonly ISettlementService _settlementService = settlementService;

    public async Task<Result<GetSettlementOverviewResponse>> Handle(GetSettlementOverviewRequest request, CancellationToken cancellationToken)
    {
        var validationResult = _validator.Validate(request);
        if (!validationResult.IsValid)
        {
            return Result<GetSettlementOverviewResponse>.Failure(validationResult.Errors);
        }

        throw new InvalidOperationException();
    }
}

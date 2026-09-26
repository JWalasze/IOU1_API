using IOU1.Application.Features.Groups.GetGroupSummary.Models.Dto;
using IOU1.Application.Features.Groups.GetGroupSummary.Models.Request;
using IOU1.Domain.Models.Results;

namespace IOU1.Application.Features.Groups.GetGroupSummary.Handler;

public interface IGetGroupSummaryHandler
{
    Task<Result<GetGroupSummaryResponse?>> Handle(GetGroupSummaryRequest request, CancellationToken cancellationToken = default);
}

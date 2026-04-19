using IOU1.Application.Features.Groups.GetGroups.Models.Dto;
using IOU1.Application.Features.Groups.GetGroups.Models.Request;
using IOU1.Domain.Models.Results;

namespace IOU1.Application.Features.Groups.GetGroups.Handler;

public interface IGetGroupsHandler
{
    Task<Result<ICollection<GetGroupsDto>>> Handle(GetGroupsRequest request, CancellationToken cancellationToken = default);
}

using IOU1.Application.Features.Groups.GetGroup.Models.Dto;
using IOU1.Application.Features.Groups.GetGroup.Models.Request;
using IOU1.Domain.Models.Results;

namespace IOU1.Application.Features.Groups.GetGroup.Handler;

public interface IGetGroupHandler
{
    Task<Result<GetGroupDto?>> Handle(GetGroupRequest request, CancellationToken cancellationToken = default);
}

using IOU1.Application.Features.Groups.AddGroup.Models.Endpoint;
using IOU1.Domain.Models.Results;

namespace IOU1.Application.Features.Groups.AddGroup.Handler;

public interface IAddGroupHandler
{
    Task<Result<AddGroupResponse?>> Handle(AddGroupRequest request, CancellationToken cancellationToken = default);
}

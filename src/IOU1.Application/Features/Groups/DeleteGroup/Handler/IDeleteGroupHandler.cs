using Application.Features.Groups.DeleteGroup.Response;
using IOU1.Application.Features.Groups.DeleteGroup.Request;
using IOU1.Domain.Models.Results;

namespace IOU1.Application.Features.Groups.DeleteGroup.Handler;

public interface IDeleteGroupHandler
{
    Task<Result<DeleteGroupResponse?>> Handle(DeleteGroupRequest request, CancellationToken cancellationToken = default);
}

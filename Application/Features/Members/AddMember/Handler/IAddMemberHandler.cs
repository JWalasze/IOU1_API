using IOU1.Application.Features.Members.AddMember.Models;
using IOU1.Domain.Models.Results;

namespace IOU1.Application.Features.Members.AddMember.Handler;

public interface IAddMemberHandler
{
    Task<Result<AddMemberDto?>> Handle(AddMemberRequest request, CancellationToken cancellationToken = default);
}

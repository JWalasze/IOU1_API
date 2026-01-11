using IOU1.Application.Mediator;

namespace Application.Features.Groups.GetGroups.Request;

public class GroupsRequest : IRequest
{
    public string Status { get; set; }

    public long UserId { get; set; } //For tests purpose
}

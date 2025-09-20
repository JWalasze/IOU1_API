using Application.Features.Groups.Response;
using Application.Mediator;

namespace Application.Features.Groups.Request;

public class GroupsRequest : IRequest<GroupsResponse>
{
    public string Status { get; set; }

    public long UserId { get; set; } //For tests purpose
}

namespace IOU1.Application.Features.Groups.GetGroup.Models.Dto;

public sealed record GetGroupMemberDto
{
    public required int UserId { get; init; }

    public required string UserName { get; init; }
}

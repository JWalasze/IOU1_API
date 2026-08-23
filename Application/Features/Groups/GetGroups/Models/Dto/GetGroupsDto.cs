namespace IOU1.Application.Features.Groups.GetGroups.Models.Dto;

public sealed record GetGroupsDto
{
    public required int GroupId { get; init; }
    public required string OwnerName { get; init; }

    public string Name { get; init; }
    public string? Description { get; init; }
}

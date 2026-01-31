namespace IOU1.Application.Features.Groups.GetGroups.Models.Dto;

public record GetGroupsDto
{
    public required long GroupId { get; init; }

    public required string OwnerName { get; init; }

    public string? Description { get; init; }
}

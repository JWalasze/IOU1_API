namespace Application.Features.Groups.Dto;

public record GetGroupsDto
{
    public required long GroupId { get; init; }

    public required string OwnerName { get; init; }

    public required string Description { get; init; }
}

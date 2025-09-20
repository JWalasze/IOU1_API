namespace Application.Features.Groups.Dtos;

public record GetGroupsDto
{
    public required long GroupId { get; init; }

    public required string OwnerName { get; init; }

    public required string Description { get; init; }
}

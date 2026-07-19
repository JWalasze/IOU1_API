namespace IOU1.Application.Features.Groups.GetGroup.Models.Dto;

public sealed record GetGroupDto
{
    public required int GroupId { get; init; }

    public required string OwnerName { get; init; }

    public string? Description { get; init; }

    public required ICollection<GetGroupMemberDto> Members { get; init; } = [];

    public ICollection<GetGroupExpenseDto> Expenses { get; init; } = [];
}

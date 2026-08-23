namespace IOU1.Application.Features.Groups.GetGroup.Models.Dto;

public sealed record GroupDto
{
    public required int GroupId { get; init; }

    public required string Title { get; set; }
    public string? Description { get; init; }
    public required string Currency { get; init; }

    public required IEnumerable<MemberDto> Members { get; init; } = [];
    public IEnumerable<ExpenseDto> Expenses { get; init; } = [];
    public IEnumerable<MemberBalanceDto> Balances { get; init; } = [];
}

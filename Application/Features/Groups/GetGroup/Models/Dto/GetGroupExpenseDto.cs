namespace IOU1.Application.Features.Groups.GetGroup.Models.Dto;

public sealed record GetGroupExpenseDto
{
    public required int ExpenseId { get; init; }

    public required int BuyerId { get; init; }

    public required decimal Amount { get; init; }

    public string? Currency { get; init; }

    public required ICollection<GetGroupExpenseShareDto> ExpenseShares { get; init; } = [];
}

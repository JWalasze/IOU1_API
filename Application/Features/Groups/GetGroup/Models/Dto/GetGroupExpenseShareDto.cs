namespace IOU1.Application.Features.Groups.GetGroup.Models.Dto;

public sealed record GetGroupExpenseShareDto
{
    public required int ExpenseShareId { get; init; }

    public required int MemberId { get; init; }

    public required decimal Amount { get; init; }

    public string? Currency { get; init; }
}

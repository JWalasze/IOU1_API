namespace IOU1.Application.Features.Groups.GetGroup.Models.Dto;

public sealed record GetGroupExpenseShareDto
{
    public required long ExpenseShareId { get; init; }

    public required long MemberId { get; init; }

    public required decimal Amount { get; init; }

    public string? Currency { get; init; }
}

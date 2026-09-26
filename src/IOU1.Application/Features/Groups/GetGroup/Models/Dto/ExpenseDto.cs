namespace IOU1.Application.Features.Groups.GetGroup.Models.Dto;

public sealed record ExpenseDto(
    int Id,
    int PayerId,
    decimal Amount,
    DateTime CreatedAt,
    IEnumerable<ExpenseShareDto> ExpenseShares);

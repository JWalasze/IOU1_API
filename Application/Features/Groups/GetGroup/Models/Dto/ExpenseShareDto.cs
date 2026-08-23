namespace IOU1.Application.Features.Groups.GetGroup.Models.Dto;

public sealed record ExpenseShareDto(
    int Id,
    int MemberId,
    decimal Amount);

namespace IOU1.Application.Features.Groups.GetGroup.Models.Dto;

public sealed record MemberBalanceDto(
    int MemberId,
    int CounterpartyId,
    decimal Amount);

namespace IOU1.Application.Features.Members.AddMember.Models;

public record AddMemberDto(
    int UserId,
    int GroupId,
    int MemberId);

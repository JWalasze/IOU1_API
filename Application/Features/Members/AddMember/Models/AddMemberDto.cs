namespace IOU1.Application.Features.Members.AddMember.Models;

public record AddMemberDto(
    long UserId,
    long GroupId,
    long MemberId);

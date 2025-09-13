namespace IOU1_API.DTOs;

public record GroupDto(
    long Id,
    string Description,
    UserDto Owner,
    List<GroupMemberDto> Members
);

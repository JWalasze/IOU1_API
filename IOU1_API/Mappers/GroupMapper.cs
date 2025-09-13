using Domain.Entities;
using IOU1_API.DTOs;

namespace IOU1_API.Mappers;

public static class GroupMapper
{
    public static GroupDto ToDto(this Group group)
    {
        return new GroupDto(
            group.Id,
            group.Description,
            group.Owner.ToDto(),
            group.Members.Select(m => m.ToDto()).ToList()
        );
    }

    public static UserDto ToDto(this User user)
    {
        return new UserDto(
            user.Id,
            user.FirstName,
            user.LastName,
            user.Email.EmailAddress // assuming Email is a ValueObject
        );
    }

    public static GroupMemberDto ToDto(this GroupMember member)
    {
        return new GroupMemberDto(
            member.Id,
            member.User.ToDto()
        );
    }
}


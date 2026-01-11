using IOU1.Domain.Base;

namespace IOU1.Domain.Entities;

public class GroupMember : Entity
{
    public Group Group { get; } = null!;

    public long MemberId { get; }
    public User User { get; } = null!;

    private GroupMember() { }

    public GroupMember(Group group, User user)
    {
        Group = group;
        User = user;
        MemberId = user.Id;
    }

    public GroupMember(long id, Group group, User user)
        : this(group, user)
    {
        Id = id;
    }

    public static GroupMember Create(
        Group group,
        User user)
    {
        return new GroupMember(
            group,
            user);
    }
}

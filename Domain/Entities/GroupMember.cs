using IOU1.Domain.Base;

namespace IOU1.Domain.Entities;

public class GroupMember : Entity
{
    public long GroupId { get; }
    public Group Group { get; } = null!;

    public long UserId { get; }
    public User User { get; } = null!;

    private GroupMember() { }

    public GroupMember(Group group, User user)
    {
        Group = group;
        GroupId = group.Id;
        User = user;
        UserId = user.Id;
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

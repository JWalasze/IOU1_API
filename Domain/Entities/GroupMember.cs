using IOU1.Domain.Base;

namespace IOU1.Domain.Entities;

public class GroupMember : Entity
{
    public int GroupId { get; }
    public Group Group { get; } = null!;

    public int UserId { get; }
    public User User { get; } = null!;

    private GroupMember() { }

    public GroupMember(Group group, User user)
    {
        ArgumentNullException.ThrowIfNull(group);
        ArgumentNullException.ThrowIfNull(user);

        Group = group;
        GroupId = group.Id;
        User = user;
        UserId = user.Id;
    }

    public GroupMember(int id, Group group, User user)
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

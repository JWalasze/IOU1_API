using IOU1.Domain.Base;
using IOU1.Domain.Entities;

namespace Domain.Entities;

public class GroupMember : Entity
{
    public long Id { get; }
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

    public GroupMember(long id, Group group, User user) : this(group, user)
    {
        Id = id;
    }
}

using Domain.Base;

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
    }
}

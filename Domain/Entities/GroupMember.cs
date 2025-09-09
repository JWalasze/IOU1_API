using Domain.Base;

namespace Domain.Entities;

public class GroupMember : Entity
{
    public long Id { get; }
    public Group Group { get; }
    public User User { get; }

    private GroupMember() { }

    public GroupMember(long id, Group group, User user)
    {
        Id = id;
        Group = group;
        User = user;
    }
}

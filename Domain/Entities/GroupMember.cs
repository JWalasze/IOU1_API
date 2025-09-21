using Domain.Base;

namespace Domain.Entities;

public class GroupMember : Entity
{
    public long Id { get; }
    public Group Group { get; } = null!;

    public long MemberId { get; }
    public User User { get; } = null!;

    private GroupMember() { }

    public GroupMember(long id, Group group, long memberId, User user)
    {
        Id = id;
        Group = group;

        User = user;
        MemberId = memberId;
    }
}

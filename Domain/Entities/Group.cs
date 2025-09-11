using Domain.Base;

namespace Domain.Entities;

public class Group : Entity
{
    public long Id { get; }
    public string Description { get; }
    public User Owner { get; }
    public ICollection<GroupMember> Members { get; }

    private Group() { }

    public Group(long id, string description, User owner)
    {
        Id = id;
        Description = description;
        Owner = owner;
    }
}

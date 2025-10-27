using Domain.Entities;
using IOU1.Domain.Base;

namespace IOU1.Domain.Entities;

public class Group : Entity
{
    public string Description { get; } = null!;
    public User Owner { get; } = null!;

    public ICollection<GroupMember> Members { get; } = [];

    private Group() { }

    public Group(string description, User owner)
    {
        Description = description;
        Owner = owner;
    }

    public Group(long id, string description, User owner) : this(description, owner)
    {
        Id = id;
    }

    public void AddNewMembers(IEnumerable<GroupMember> newMembers)
    {
        foreach (var newMember in newMembers)
        {
            if (!Members.Any(m => m.MemberId == newMember.MemberId))
            {
                Members.Add(newMember);
            }
        }
    }

    public void AddNewMember(GroupMember newMember)
    {
        if (!Members.Any(m => m.MemberId == newMember.MemberId))
        {
            Members.Add(newMember);
        }
    }
}

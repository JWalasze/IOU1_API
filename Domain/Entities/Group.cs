using IOU1.Domain.Base;

namespace Domain.Entities;

public class Group : Entity
{
    public long Id { get; }
    public string Description { get; } = null!;
    public User Owner { get; } = null!;

    public ICollection<GroupMember> Members { get; } = [];

    private Group() { }

    public Group(string description, User owner)
    {
        Description = description;
        Owner = owner;
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
}

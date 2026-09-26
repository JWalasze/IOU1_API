using IOU1.Domain.Base;
using IOU1.Domain.Exceptions;

namespace IOU1.Domain.Entities;

public class Group : Entity
{
    public const int NameMaxLength = 50;
    public const int DescriptionMaxLength = 300;

    public string Name { get; private set; } = null!;
    public string? Description { get; private set; } = null!;

    public int OwnerId { get; private set; }
    public User Owner { get; private set; } = null!;

    public string CurrencyKey { get; private set; } = null!;
    public Currency Currency { get; private set; } = null!;

    public ICollection<GroupMember> Members { get; } = [];

    private Group() { }

    public Group(string name, string? description, User owner, Currency currency, ICollection<GroupMember> members)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new CreateGroupException($"Name cannot be null or empty.");

        Name = name;
        Description = description;
        Owner = owner;
        Currency = currency;
        CurrencyKey = currency.Key;
        Members = members;

        if (!Members
            .Select(m => m.Id)
            .Contains(owner.Id))
        {
            var ownerMember = GroupMember.Create(this, owner);
            Members.Add(ownerMember);
        }
    }

    public Group(string name, string? description, User owner, Currency currency, ICollection<User> members)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new CreateGroupException($"Name cannot be null or empty.");

        Name = name;
        Description = description;
        Owner = owner;
        Currency = currency;
        CurrencyKey = currency.Key;
        Members = [.. members.Select(member => GroupMember.Create(this, member))];

        if (!Members
            .Select(m => m.User.Id)
            .Contains(owner.Id))
        {
            var ownerMember = GroupMember.Create(this, owner);
            Members.Add(ownerMember);
        }
    }

    public void AddNewMembers(IEnumerable<GroupMember> newMembers)
    {
        foreach (var newMember in newMembers)
        {
            if (!Members.Any(m => m.UserId == newMember.UserId))
            {
                Members.Add(newMember);
            }
        }
    }

    public void AddNewMember(GroupMember newMember)
    {
        if (!Members.Any(m => m.UserId == newMember.UserId))
        {
            Members.Add(newMember);
        }
    }

    public static Group Create(
        string name,
        string? description,
        User owner,
        Currency currency,
        ICollection<GroupMember> members)
    {
        return new Group(
            name,
            description,
            owner,
            currency,
            members);
    }

    public static Group Create(
        string name,
        string? description,
        User owner,
        Currency currency,
        ICollection<User> users)
    {
        return new Group(
            name,
            description,
            owner,
            currency,
            users);
    }
}

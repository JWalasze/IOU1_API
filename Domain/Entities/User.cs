using Domain.ValueObjects;
using IOU1.Domain.Base;
using IOU1.Domain.Entities;

namespace Domain.Entities;

public class User : Entity
{
    public string FirstName { get; } = null!;
    public string LastName { get; } = null!;
    public Email Email { get; } = null!;
    public DateTime CreatedAt { get; }
    public string Login { get; } = null!;
    public string HashedPassword { get; } = null!;

    public ICollection<Group> OwnedGroups { get; } = [];
    public ICollection<GroupMember> MemberGroups { get; } = [];

    public string FullName => $"{FirstName} {LastName}";

    private User() { }

    public User(
        long id, 
        string firstName, 
        string lastName, 
        Email email, 
        string login, 
        string hashedPassword)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Login = login;
        HashedPassword = hashedPassword;
    }
}

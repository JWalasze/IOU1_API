using Domain.ValueObjects;

namespace Domain.Entities;

public class User
{
    public long Id { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public Email Email { get; }
    public DateTime CreatedAt { get; }
    public string Login { get; }
    public string HashedPassword { get; }
    public ICollection<Group> OwnedGroups { get; } = [];
    public ICollection<GroupMember> MemberGroups { get; } = [];

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

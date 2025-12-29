using Domain.Entities;
using IOU1.Domain.Base;
using IOU1.Domain.Exceptions;
using IOU1.Domain.Services.Crypto;
using IOU1.Domain.ValueObjects;

namespace IOU1.Domain.Entities;

public class User : Entity
{
    public const int SaltBytesMaxLength = 64;
    public const int HashBytesMaxLength = 64;

    public string FirstName { get; } = null!;
    public string LastName { get; } = null!;
    public Email Email { get; } = null!;
    public DateTime CreatedAt { get; }
    public string Login { get; } = null!;
    public string PasswordHash { get; } = null!;
    public string PasswordSalt { get; } = null!;

    public ICollection<Group> OwnedGroups { get; } = [];
    public ICollection<GroupMember> MemberGroups { get; } = [];

    public string FullName => $"{FirstName} {LastName}";

    //private User() { }

    public User(
        long id,
        string firstName,
        string lastName,
        Email email,
        string login,
        string passwordHash,
        string passwordSalt,
        DateTime createdAt
    )
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Login = login;
        PasswordHash = passwordHash;
        PasswordSalt = passwordSalt;
        CreatedAt = createdAt;
    }

    public User(
        string firstName,
        string lastName,
        Email email,
        string login,
        string password,
        IPasswordHasher passwordHasher)
    {
        if (string.IsNullOrWhiteSpace(firstName))
        {
            throw new CreatingUserException("First name cannot be empty.");
        }

        FirstName = firstName;

        if (string.IsNullOrWhiteSpace(lastName))
        {
            throw new CreatingUserException("Last name cannot be empty.");
        }

        LastName = lastName;

        if (string.IsNullOrWhiteSpace(login))
        {
            throw new CreatingUserException("Login cannot be empty.");
        }

        Login = login;

        if (string.IsNullOrWhiteSpace(password))
        {
            throw new CreatingUserException("Password cannot be empty.");
        }

        PasswordSalt = passwordHasher.GenerateSalt();
        PasswordHash = passwordHasher.GenerateHash(password, PasswordSalt);
        Email = email;
        CreatedAt = DateTime.UtcNow;
    }

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
        PasswordHash = hashedPassword;
    }
}

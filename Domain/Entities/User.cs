using IOU1.Domain.Base;
using IOU1.Domain.Exceptions;
using IOU1.Domain.Services.Crypto;
using IOU1.Domain.ValueObjects;

namespace IOU1.Domain.Entities;

public class User : Entity
{
    public const int SaltBytesMaxLength = 64;
    public const int HashBytesMaxLength = 64;

    public string FirstName { get; private set; } = null!;
    public string LastName { get; private set; } = null!;
    public Email Email { get; private set; } = null!;
    public DateTime CreatedAt { get; private set; }
    public string Login { get; private set; } = null!;
    public string PasswordHash { get; private set; } = null!;
    public string PasswordSalt { get; private set; } = null!;
    public bool IsDeleted { get; private set; }

    public ICollection<Group> OwnedGroups { get; } = [];
    public ICollection<GroupMember> MemberGroups { get; } = [];

    public string FullName => $"{FirstName} {LastName}";

    private User() { }

    private User(
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

    private User(
        string firstName,
        string lastName,
        Email email,
        DateTime createdAt,
        string login,
        string passwordHash,
        string passwordSalt)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        CreatedAt = createdAt;
        Login = login;
        PasswordHash = passwordHash;
        PasswordSalt = passwordSalt;
    }

    public static User Create(
        string firstName,
        string lastName,
        Email email,
        string login,
        string passwordHash,
        string passwordSalt,
        DateTime createdAt
    )
    {
        return new User(
            firstName,
            lastName,
            email,
            createdAt,
            login,
            passwordHash,
            passwordSalt);
    }

    public static User Create(
        string firstName,
        string lastName,
        Email email,
        string login,
        string password,
        IPasswordHasher passwordHasher)
    {
        return new User(
            firstName,
            lastName,
            email,
            login,
            password,
            passwordHasher);
    }

    public void Delete()
    {
        IsDeleted = true;
    }
}

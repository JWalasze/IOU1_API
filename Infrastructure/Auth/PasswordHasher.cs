using IOU1.Application.Features.Auth;
using IOU1.Domain.Entities;
using IOU1.Domain.Services;
using System.Security.Cryptography;
using System.Text;

namespace IOU1.Infrastructure.Auth;

public class PasswordHasher : IPasswordHasher
{
    public string GenerateHash(string source, string salt)
    {
        var hash = Rfc2898DeriveBytes.Pbkdf2(
            Encoding.UTF8.GetBytes(source),
            Encoding.UTF8.GetBytes(salt),
            350000,
            HashAlgorithmName.SHA512,
            64);

        var hashedSource = Convert.ToBase64String(hash);

        return hashedSource;
    }

    public string GenerateSalt()
    {
        var rng = RandomNumberGenerator.Create();

        byte[] salt = new byte[User.SaltBytesMaxLength];

        rng.GetBytes(salt);

        string cryptSalt = Convert.ToBase64String(salt);

        return cryptSalt;
    }

    public string Hash(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool Verify(string inputPassword, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(inputPassword, hashedPassword);
    }
}

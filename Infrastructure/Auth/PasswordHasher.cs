using IOU1.Domain.Entities;
using IOU1.Domain.Services.Crypto;
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

        return Convert.ToBase64String(hash);
    }

    public string GenerateSalt()
    {
        var rng = RandomNumberGenerator.Create();
        var salt = new byte[User.SaltBytesMaxLength];

        rng.GetBytes(salt);

        return Convert.ToBase64String(salt);
    }
}

using System.Security.Cryptography;
using System.Text;

namespace IOU1.Infrastructure.Utils;

public static class CryptoUtil
{
    public static string GenrateHash(string source, string salt)
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
}

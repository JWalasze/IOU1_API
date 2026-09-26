using IOU1.Domain.Services.Crypto;
using System.Security.Cryptography;

namespace IOU1.Infrastructure.Auth;

public class PasswordComparer : IPasswordComparer
{
    public bool Compare(string hashedPassword1, string hashedPassword2)
    {
        return CryptographicOperations.FixedTimeEquals(Convert.FromBase64String(hashedPassword1), Convert.FromBase64String(hashedPassword2));
    }
}

namespace IOU1.Domain.Services.Crypto;

public interface IPasswordHasher
{
    string GenerateHash(string source, string salt);

    string GenerateSalt();
}

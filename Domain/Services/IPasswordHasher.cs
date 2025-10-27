namespace IOU1.Domain.Services;

public interface IPasswordHasher
{
    string Hash(string password);

    bool Verify(string inputPassword, string hashedPassword);
}

namespace IOU1.Domain.Services.Crypto;

public interface IPasswordComparer
{
    bool Compare(string hashedPassword1, string hashedPassword2);
}

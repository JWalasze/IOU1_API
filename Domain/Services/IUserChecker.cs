namespace IOU1.Domain.Services;

public interface IUserChecker
{
    Task<bool> IsEmailTaken(string email);

    Task<bool> IsLoginTaken(string login);
}

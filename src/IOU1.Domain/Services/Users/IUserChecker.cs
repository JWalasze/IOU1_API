namespace IOU1.Domain.Services.Users;

public interface IUserChecker
{
    Task<bool> IsEmailTaken(string email);

    Task<bool> IsLoginTaken(string login);
}

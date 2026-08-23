namespace IOU1.Domain.Models.Auth.User;

public interface IAuthUser
{
    int Id { get; }
    string Login { get; }

    void SetId(int id);
    void SetLogin(string login);
}

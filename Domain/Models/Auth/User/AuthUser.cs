namespace IOU1.Domain.Models.Auth.User;

public sealed record AuthUser : IAuthUser
{
    public int Id { get; private set; }
    public string Login { get; private set; } = null!;

    public void SetId(int id)
    {
        if (Id > 0)
            throw new InvalidOperationException("Id has already been set and cannot be changed.");

        Id = id;
    }

    public void SetLogin(string login)
    {
        if (!string.IsNullOrWhiteSpace(Login))
            throw new InvalidOperationException("Login has already been set and cannot be changed.");

        Login = login;
    }
}

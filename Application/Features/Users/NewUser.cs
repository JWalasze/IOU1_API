namespace IOU1.Application.Features.Users;

public record NewUser(
    string FirstName,
    string LastName,
    string Email,
    string Login,
    string Password);

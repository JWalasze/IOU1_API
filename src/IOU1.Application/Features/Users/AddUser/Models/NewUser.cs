namespace IOU1.Application.Features.Users.AddUser.Models;

public record NewUser(
    string FirstName,
    string LastName,
    string Email,
    string Login,
    string Password);

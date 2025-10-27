using Application.Mediator;

namespace IOU1.Application.Features.Users;

public sealed record AddUserRequest(
    string FirstName,
    string LastName,
    string Email,
    string Login,
    string Password) : IRequest;

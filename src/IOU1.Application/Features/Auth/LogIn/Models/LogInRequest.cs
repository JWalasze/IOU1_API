using IOU1.Application.Mediator;

namespace IOU1.Application.Features.Auth.LogIn.Models;

public record LogInRequest(string Login, string Password) : IRequest;

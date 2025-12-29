namespace IOU1.Application.Features.Auth.LogIn.Models;

public record LogInResponse : EndpointResponse
{
    public string? Token { get; init; }
}

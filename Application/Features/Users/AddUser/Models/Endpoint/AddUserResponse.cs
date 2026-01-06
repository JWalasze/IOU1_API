namespace IOU1.Application.Features.Users.AddUser.Models.Endpoint;

public sealed record AddUserResponse(
    string FirstName,
    string LastName,
    string Email,
    string Login) : EndpointResponse
{
    public AddUserResponse() : this(default!, default!, default!, default!) { }
}

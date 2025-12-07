namespace IOU1.Application.Features.Users.Models.Endpoint;

public record AddUserResponse : EndpointResponse
{
    public AddUserResponse() { }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Login { get; set; }
}

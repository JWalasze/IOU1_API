using Application.Mediator;

namespace Application;

public record EndpointResponse : IResponse
{
    public string? ErrorMessage { get; set; }
    public bool IsSuccess { get; set; }
}

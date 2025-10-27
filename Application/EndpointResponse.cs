using IOU1.Application.Mediator;

namespace IOU1.Application;

public record EndpointResponse : IResponse
{
    public string? ErrorMessage { get; set; }
    public bool IsSuccess { get; set; }
}

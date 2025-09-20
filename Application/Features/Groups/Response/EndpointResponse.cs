using Application.Mediator;

namespace Application.Features.Groups.Response;

public record EndpointResponse : IResponse
{
    public string? ErrorMessage {  get; set; }
    public bool IsSuccess { get; set; }
}

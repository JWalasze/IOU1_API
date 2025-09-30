using Application.Mediator;

namespace Application.Features.Groups.GetGroups.Response;

public record EndpointResponse : IResponse
{
    public string? ErrorMessage { get; set; }
    public bool IsSuccess { get; set; }
}

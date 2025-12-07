using IOU1.Application.Mediator;
using IOU1.Domain.Models;

namespace IOU1.Application;

public record EndpointResponse : IResponse
{
    public string? ErrorMessage { get; init; }

    public bool IsSuccess => !Errors.Any();

    public IEnumerable<ProblemDetails> Errors { get; init; } = [];
}

using IOU1.Application.Mediator;
using IOU1.Domain.Models.Results;

namespace IOU1.Application;

//WHat if it would be a generic version like EndpointResponse<TData>
public record EndpointResponse : IResponse
{
    public string? ErrorMessage { get; init; }

    public bool IsSuccess => !Errors.Any();

    public IEnumerable<ProblemDetails> Errors { get; init; } = [];
}

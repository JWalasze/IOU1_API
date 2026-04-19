using IOU1.Domain.Models.Results;

namespace IOU1.Application;

public class HandlerResponse<T>
    : IHandlerResponse<T>
    where T : class
{
    public T? Data { get; init; }

    public string? ErrorMessage { get; init; }

    public bool IsSuccess => !Errors.Any();

    public IEnumerable<ProblemDetails> Errors { get; init; } = [];
}

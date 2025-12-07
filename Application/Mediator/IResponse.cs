using IOU1.Domain.Models;

namespace IOU1.Application.Mediator;

public interface IResponse
{
    string? ErrorMessage { get; init; }

    bool IsSuccess { get; }

    IEnumerable<ProblemDetails> Errors { get; init; }
}

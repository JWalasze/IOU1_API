using IOU1.Application.Mediator;
using IOU1.Domain.Models;

namespace IOU1.Application;

public interface IHandlerResponse<out T> : IResponse
{
    T? Data { get; }

    IEnumerable<ProblemDetails> Errors { get; }
}

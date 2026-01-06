using IOU1.Application.Mediator;

namespace IOU1.Application;

public interface IHandlerResponse<out T> : IResponse
{
    T? Data { get; }
}

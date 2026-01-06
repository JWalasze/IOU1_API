using Application.Mediator;

namespace IOU1.Application.Mediator;

public interface IRequestHandler<TRequest, TResponse>
    where TRequest : class, IRequest
    where TResponse : class, IResponse
{
    Task<IHandlerResponse<TResponse>> Handle(TRequest request, CancellationToken cancellationToken = default);
}

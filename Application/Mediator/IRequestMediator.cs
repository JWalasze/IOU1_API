namespace IOU1.Application.Mediator;

public interface IRequestMediator
{
    Task<IHandlerResponse<TResponse>> Send<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken = default)
        where TRequest : class, IRequest
        where TResponse : class, IResponse;
}

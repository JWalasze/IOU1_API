namespace Application.Mediator;

public interface IRequestHandler<TRequest, TResponse>
    where TRequest : IRequest where TResponse : IResponse
{
    Task<TResponse> Handle(TRequest request);
}

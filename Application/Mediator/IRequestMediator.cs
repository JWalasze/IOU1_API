namespace Application.Mediator;

public interface IRequestMediator
{
    Task<TResponse> Send<TRequest, TResponse>(TRequest request) where TRequest : IRequest<TResponse>;
}

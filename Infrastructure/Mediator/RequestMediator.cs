using Application.Mediator;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Mediator;

public class RequestMediator(IServiceProvider serviceProvider) : IRequestMediator
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public async Task<TResponse> Send<TRequest, TResponse>(TRequest request)
        where TRequest : IRequest where TResponse : IResponse
    {
        var service = _serviceProvider.GetRequiredService<IRequestHandler<TRequest, TResponse>>();
        return await service.Handle(request);
    }
}

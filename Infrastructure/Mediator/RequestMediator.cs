using Application.Mediator;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Mediator;

public class RequestMediator(IServiceProvider serviceProvider) : IRequestMediator
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public async Task<TResponse> Send<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken = default)
        where TRequest : IRequest where TResponse : IResponse
    {
        using var scope = _serviceProvider.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IRequestHandler<TRequest, TResponse>>();
        return await service.Handle(request, cancellationToken);
    }
}

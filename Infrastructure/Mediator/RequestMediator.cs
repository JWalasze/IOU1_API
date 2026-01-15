using IOU1.Application;
using IOU1.Application.Mediator;
using Microsoft.Extensions.DependencyInjection;

namespace IOU1.Infrastructure.Mediator;

public class RequestMediator(IServiceProvider serviceProvider) : IRequestMediator
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public async Task<IHandlerResponse<TResponse>> Send<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken = default)
        where TRequest : class, IRequest
        where TResponse : class, IResponse
    {
        var service = _serviceProvider.GetRequiredService<IRequestHandler<TRequest, TResponse>>();
        return await service.Handle(request, cancellationToken);
    }
}

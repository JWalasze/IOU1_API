using IOU1.Application.Messages;
using MassTransit;

namespace IOU1.Infrastructure.Messages
{
    public class ServiceBus(IPublishEndpoint publishEndpoint, ISendEndpointProvider sendEndpointProvider) : IServiceBus
    {
        private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;
        private readonly ISendEndpointProvider _sendEndpointProvider = sendEndpointProvider;

        public async Task PublishAsync<T>(T @event, CancellationToken cancellation = default)
        {
            await _publishEndpoint.Publish(@event, cancellation);
        }

        public async Task SendAsync<T>(T message, string queueName, CancellationToken cancellation = default)
        {
            var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{queueName}"));
            await endpoint.Send(message, cancellation);
        }
    }
}

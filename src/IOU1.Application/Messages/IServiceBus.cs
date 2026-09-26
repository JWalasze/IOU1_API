namespace IOU1.Application.Messages;

public interface IServiceBus
{
    Task PublishAsync<T>(T @event, CancellationToken cancellation = default);
    Task SendAsync<T>(T message, string queueName, CancellationToken cancellation = default);
}

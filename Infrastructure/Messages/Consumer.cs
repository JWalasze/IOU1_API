using IOU1.Application.Features.Users.AddUser;
using MassTransit;

namespace IOU1.Infrastructure.Messages;


public class ProductAddedEventConsumer : IConsumer<TestMessage>
{
    public Task Consume(ConsumeContext<TestMessage> context)
    {
        Console.WriteLine($"Received Event: {context.Message.Mess}");
        return Task.CompletedTask;
    }
}


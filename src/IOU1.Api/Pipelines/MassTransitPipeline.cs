namespace IOU1.Api.Pipelines;

public static class MassTransitPipeline
{
    public static void AddMassTransitPipeline(this WebApplicationBuilder builder)
    {
        //builder.Services.AddMassTransit(x =>
        //{
        //    x.AddConsumer<ProductAddedEventConsumer>();
        //    x.UsingRabbitMq((context, cfg) =>
        //    {

        //        cfg.Host("localhost", "/", h => {
        //            h.Username("kalo");
        //            h.Password("kalo");
        //        });

        //        //cfg.ConfigureEndpoints(context);
        //        cfg.ReceiveEndpoint("TestQueue",
        //            e => { e.ConfigureConsumer<ProductAddedEventConsumer>(context); });
        //    });
        //});
    }
}

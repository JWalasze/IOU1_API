namespace IOU1.Api.Pipelines;

public static class WebPipeline
{
    public static void AddWebPipeline(this WebApplicationBuilder builder)
    {
        builder.Services.AddSignalR();
        builder.Services.AddControllers();
    }
}

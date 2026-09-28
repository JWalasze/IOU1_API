namespace IOU1.Api.Pipelines;

public static class CorsPipeline
{
    public static void AddCorsPipeline(this WebApplicationBuilder builder)
    {
        builder.Services.AddCors(options =>
        {
            options.AddPolicy(
                "DevCors",
                p =>
                    p.WithOrigins(
                            "http://localhost:4200",
                            "https://localhost:4200",
                            "http://localhost:5173",
                            "https://localhost:5173"
                        )
                        .AllowAnyHeader()
                        .AllowAnyMethod()
            );
        });
    }
}

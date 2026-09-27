using Scalar.AspNetCore;

namespace IOU1.Api.Pipelines;

public static class DevPipeline
{
    public static void AddDevPipeline(this WebApplication app)
    {
        if (!app.Environment.IsDevelopment())
            return;

        app.MapOpenApi();
        app.MapScalarApiReference();
        app.UseCors(builder =>
            builder
                .WithOrigins("http://localhost:5173")
                .AllowAnyHeader()
                .AllowAnyMethod());
    }
}

using IOU1.Persistance.Context;
using Microsoft.EntityFrameworkCore;

namespace IOU1.Api.Pipelines;

public static class DatabasePipeline
{
    public static void AddDatabasePipeline(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<IOU1Context>(opt =>
            opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
                .EnableSensitiveDataLogging()
        );
    }
}

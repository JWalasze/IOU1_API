using IOU1.Application.Options;

namespace IOU1.Api.Pipelines;

public static class OptionsPipeline
{
    public static void AddOptionsPipeline(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<LinkInvitation>(builder.Configuration.GetSection("LinkInvitation"));
        builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("Jwt"));
    }
}

using IOU1.Domain.Services.Crypto;
using IOU1.Infrastructure.Auth;

namespace IOU1.Api.Pipelines;

public static class SingletonServicesPipeline
{
    public static void AddSingletonServicesPipeline(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IPasswordHasher, PasswordHasher>();
        builder.Services.AddSingleton<IPasswordComparer, PasswordComparer>();

        builder.Services.AddSingleton<ITokenProvider, JwtTokenProvider>();
    }
}

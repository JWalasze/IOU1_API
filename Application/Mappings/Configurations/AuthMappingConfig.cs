using IOU1.Application.Features.Auth.LogIn.Models;
using IOU1.Domain.Models.Auth;
using IOU1.Domain.Models.Results;
using Mapster;

namespace IOU1.Application.Mappings.Configurations;

public class AuthMappingConfig : IMappingConfiguration
{
    public void Init()
    {
        TypeAdapterConfig<Result<Token>, LogInResponse>
            .NewConfig()
            .Map(dest => dest.Token, src => src.Data != null ? src.Data.Value : null);
    }
}

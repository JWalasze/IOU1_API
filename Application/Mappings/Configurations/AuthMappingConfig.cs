using IOU1.Application.Features.Auth;
using IOU1.Application.Features.Auth.LogIn.Models;
using IOU1.Domain.Models;
using Mapster;

namespace IOU1.Application.Mappings.Configurations;

public class AuthMappingConfig : IMappingConfiguration
{
    public void Init()
    {
        TypeAdapterConfig<Result<Token>, LogInResponse>
            .NewConfig()
            .Map(dest => dest.Token, src => src.Data != null ? src.Data.Value : null)
            .Map(dest => dest.IsSuccess, src => src.IsSuccess)
            .Map(dest => dest.ErrorMessage, src => src.ErrorMessage);
    }
}

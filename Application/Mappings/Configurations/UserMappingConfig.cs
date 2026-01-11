using IOU1.Application.Features.Users.AddUser.Models.Endpoint;
using IOU1.Application.Features.Users.DeleteUser.Models.Endpoint;
using IOU1.Application.Mappings.Extensions;
using IOU1.Domain.Entities;
using IOU1.Domain.Models;
using Mapster;

namespace IOU1.Application.Mappings.Configurations;

public class UserMappingConfig : IMappingConfiguration
{
    public void Init()
    {
        TypeAdapterConfig<User, AddUserResponse>
            .NewConfig()
            .Map(dest => dest.FirstName, src => src.FirstName)
            .Map(dest => dest.LastName, src => src.LastName)
            .Map(dest => dest.Email, src => src.Email.EmailAddress)
            .Map(dest => dest.Login, src => src.Login);

        TypeAdapterConfig<Result<User>, AddUserResponse>
            .NewConfig()
            .MapEndpointResponseInfo()
            .Map(dest => dest.FirstName, src => src.Data != null ? src.Data.FirstName : null)
            .Map(dest => dest.LastName, src => src.Data != null ? src.Data.LastName : null)
            .Map(dest => dest.Email, src => src.Data != null ? src.Data.Email : null)
            .Map(dest => dest.Login, src => src.Data != null ? src.Data.Login : null);

        TypeAdapterConfig<string, DeleteUserResponse>
            .NewConfig()
            .Map(dest => dest.Message, src => src);
    }
}

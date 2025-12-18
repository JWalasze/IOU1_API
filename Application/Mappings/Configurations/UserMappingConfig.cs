using IOU1.Application.Features.Users.AddUser.Models.Endpoint;
using IOU1.Domain.Entities;
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
    }
}

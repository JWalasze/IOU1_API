using Application.Features.Groups.GetGroups.Dto;
using Application.Features.Groups.GetGroups.Response;
using Mapster;

namespace IOU1.Application.Mappings.Configurations;

public class GroupMappingConfig : IMappingConfiguration
{
    public void Init()
    {
        TypeAdapterConfig<GetGroupsDto, GroupInfoResponse>
            .NewConfig()
            .Map(dest => dest.Id, src => src.GroupId)
            .Map(dest => dest.Description, src => src.Description)
            .Map(dest => dest.OwnerName, src => src.OwnerName);

        TypeAdapterConfig<IEnumerable<GetGroupsDto>, IEnumerable<GroupInfoResponse>>
            .NewConfig()
            .Map(dest => dest, src => src.Select(s => s.Adapt<GroupInfoResponse>()));
    }
}
using Application.Features.Groups.GetGroups.Response;
using IOU1.Application.Features.Groups.GetGroups.Dto;
using IOU1.Domain.Models;
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

        TypeAdapterConfig<ICollection<GetGroupsDto>, ICollection<GroupInfoResponse>>
            .NewConfig()
            .Map(dest => dest, src => src.Select(s => s.Adapt<GroupInfoResponse>()));

        TypeAdapterConfig<Result<ICollection<GetGroupsDto>>, GroupsResponse>
            .NewConfig()
            .Map(dest => dest.GroupInfoResponse, src => src.Data != null ? src.Data.Select(s => s.Adapt<GroupInfoResponse>()) : new List<GroupInfoResponse>());
    }
}
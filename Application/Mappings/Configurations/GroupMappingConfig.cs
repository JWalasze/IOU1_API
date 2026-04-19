namespace IOU1.Application.Mappings.Configurations;

public class GroupMappingConfig : IMappingConfiguration
{
    public void Init()
    {
        //TypeAdapterConfig<GetGroupsDto, GroupInfoResponse>
        //    .NewConfig()
        //    .Map(dest => dest.Id, src => src.GroupId)
        //    .Map(dest => dest.Description, src => src.Description)
        //    .Map(dest => dest.OwnerName, src => src.OwnerName);

        //TypeAdapterConfig<ICollection<GetGroupsDto>, ICollection<GroupInfoResponse>>
        //    .NewConfig()
        //    .Map(dest => dest, src => src.Select(s => s.Adapt<GroupInfoResponse>()));

        //    TypeAdapterConfig<Result<ICollection<GetGroupsDto>>, GetGroupsResponse>
        //        .NewConfig()
        //        .Map(dest => dest.Groups, src => src.Data != null ? src.Data.Select(s => s) : new List<GetGroupsDto>());
        //}
    }
}
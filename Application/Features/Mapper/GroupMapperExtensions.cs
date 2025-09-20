using Application.Features.Groups.Dtos;
using Application.Features.Groups.Response;

namespace Application.Features.Mapper;

public static class GroupMapperExtensions
{
    public static List<GroupInfoResponse> MapToDto(this IEnumerable<GetGroupsDto> source)
    {
        ArgumentNullException.ThrowIfNull(source);

        var result = new List<GroupInfoResponse>();
        foreach (var item in source) 
        {
            result.Add(new GroupInfoResponse(
                Id: item.GroupId,
                Description: item.Description,
                OwnerName: item.OwnerName));
        }

        return result;
    }
}

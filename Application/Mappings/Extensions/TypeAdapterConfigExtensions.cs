using IOU1.Domain.Models.Results;
using Mapster;

namespace IOU1.Application.Mappings.Extensions;

public static class TypeAdapterConfigExtensions
{
    public static TypeAdapterSetter<Result<TSource>, TDest> MapEndpointResponseInfo<TSource, TDest>(
        this TypeAdapterSetter<Result<TSource>, TDest> config)
        where TSource : class
        where TDest : EndpointResponse
    {
        config
            .Map(dest => dest.IsSuccess, src => src.IsSuccess)
            .Map(dest => dest.ErrorMessage, src => src.ErrorMessage)
            .Map(dest => dest.Errors, src => src.Errors);

        return config;
    }
}

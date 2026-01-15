using FluentValidation.Results;
using Mapster;

namespace IOU1.Application.Mappings.Configurations;

public class ValidationResultMappingConfig : IMappingConfiguration
{
    public void Init()
    {
        TypeAdapterConfig<ValidationResult, EndpointResponse>
            .NewConfig()
            .Map(dest => dest.Errors, src => src.Errors)
            .Map(dest => dest.ErrorMessage, src => "Validation errors occured!")
            .Map(dest => dest.IsSuccess, src => src.IsValid);

        TypeAdapterConfig<ValidationResult, ValidateResult>
            .NewConfig()
            .Map(dest => dest.Errors, src => src.Errors);
    }
}

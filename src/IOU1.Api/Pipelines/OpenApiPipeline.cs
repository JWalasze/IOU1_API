using Microsoft.OpenApi;

namespace IOU1.Api.Pipelines;

public static class OpenApiPipeline
{
    public static void AddOpenApiPipeline(this WebApplicationBuilder builder)
    {
        builder.Services.AddOpenApi(options =>
        {
            options.AddDocumentTransformer(
                (document, context, cancellationToken) =>
                {
                    document.Components ??= new OpenApiComponents();
                    document.Components.SecuritySchemes ??=
                        new Dictionary<string, IOpenApiSecurityScheme>();
                    document.Components.SecuritySchemes["Bearer"] = new OpenApiSecurityScheme
                    {
                        Type = SecuritySchemeType.Http,
                        Scheme = "bearer",
                        BearerFormat = "JWT",
                        In = ParameterLocation.Header,
                        Description = "Enter: Bearer {your JWT token}",
                    };

                    document.Security ??= new List<OpenApiSecurityRequirement>();
                    document.Security.Add(
                        new OpenApiSecurityRequirement
                        {
                            [new OpenApiSecuritySchemeReference("Bearer", document)] = [],
                        }
                    );

                    return Task.CompletedTask;
                }
            );
        });
    }
}

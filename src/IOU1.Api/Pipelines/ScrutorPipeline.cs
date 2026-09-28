using FluentValidation;
using IOU1.Application;
using Scrutor;

namespace IOU1.Api.Pipelines;

public static class ScrutorPipeline
{
    public static void AddScrutorPipeline(this WebApplicationBuilder builder)
    {
        //Validators
        builder.Services.Scan(scan =>
            scan.FromAssemblies(typeof(EndpointResponse).Assembly)
                .AddClasses(filter => filter.AssignableTo(typeof(IValidator<>)))
                .UsingRegistrationStrategy(RegistrationStrategy.Throw)
                .AsImplementedInterfaces(i =>
                    i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IValidator<>)
                )
                .WithScopedLifetime()
        );

        //Handlers
        builder.Services.Scan(scan =>
            scan.FromAssemblies(typeof(EndpointResponse).Assembly)
                .AddClasses(filter => filter.Where(f => f.Name.EndsWith("Handler")))
                .UsingRegistrationStrategy(RegistrationStrategy.Throw)
                .AsImplementedInterfaces()
                .WithTransientLifetime()
        );

        //Queries
        builder.Services.Scan(scan =>
            scan.FromAssemblies(typeof(EndpointResponse).Assembly)
                .AddClasses(filter => filter.Where(f => f.Name.EndsWith("Query")))
                .UsingRegistrationStrategy(RegistrationStrategy.Throw)
                .AsImplementedInterfaces()
                .WithScopedLifetime()
        );
    }
}

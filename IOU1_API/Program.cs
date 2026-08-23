using Domain.RepoInterfaces;
using Elastic.Channels;
using Elastic.CommonSchema.Serilog;
using Elastic.Ingest.Elasticsearch;
using Elastic.Ingest.Elasticsearch.DataStreams;
using Elastic.Serilog.Sinks;
using FluentValidation;
using IOU1.API.Middlewares;
using IOU1.Application;
using IOU1.Application.Features.Auth;
using IOU1.Application.Mappings;
using IOU1.Application.Options;
using IOU1.Application.Services.Balances;
using IOU1.Application.Services.Expenses;
using IOU1.Application.Services.Groups;
using IOU1.Application.Services.Groups.Summary;
using IOU1.Application.Services.Invitations.General;
using IOU1.Application.Services.Members;
using IOU1.Application.Services.Notifications;
using IOU1.Application.Services.Settlements;
using IOU1.Application.Services.Users;
using IOU1.Application.Services.Users.Checker;
using IOU1.Domain.Entities;
using IOU1.Domain.Models.Auth.User;
using IOU1.Domain.RepoInterfaces;
using IOU1.Domain.Services.Crypto;
using IOU1.Domain.Services.Users;
using IOU1.Domain.UnitOfWork;
using IOU1.Infrastructure.Auth;
using IOU1.Infrastructure.Notifications;
using IOU1.Infrastructure.Repositories;
using IOU1.Infrastructure.UnitOfWork;
using IOU1.Persistance.Context;
using Mapster;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Scrutor;
using Serilog;
using System.Text;

namespace IOU1.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder();

        builder.Services.AddDbContext<IOU1Context>(opt => opt
            .UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
            .EnableSensitiveDataLogging());

        #region Scutor dependencies
        //Validators
        builder.Services
            .Scan(scan => scan
            .FromAssemblies(typeof(ValidateResult).Assembly)
            .AddClasses(
                filter => filter.AssignableTo(typeof(IValidator<>)))
            .UsingRegistrationStrategy(RegistrationStrategy.Throw)
            .AsImplementedInterfaces(i =>
                i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IValidator<>))
            .WithScopedLifetime());

        //Handlers
        builder.Services
            .Scan(scan => scan
            .FromAssemblies(typeof(ValidateResult).Assembly)
            .AddClasses(
                filter => filter.Where(f => f.Name.EndsWith("Handler")))
            .UsingRegistrationStrategy(RegistrationStrategy.Throw)
            .AsImplementedInterfaces()
            .WithTransientLifetime());

        //Queries
        builder.Services
            .Scan(scan => scan
            .FromAssemblies(typeof(ValidateResult).Assembly)
            .AddClasses(
                filter => filter.Where(f => f.Name.EndsWith("Query")))
            .UsingRegistrationStrategy(RegistrationStrategy.Throw)
            .AsImplementedInterfaces()
            .WithScopedLifetime());
        #endregion

        #region SingletonServices
        builder.Services.AddSingleton<IPasswordHasher, PasswordHasher>();
        builder.Services.AddSingleton<IPasswordComparer, PasswordComparer>();

        builder.Services.AddSingleton<ITokenProvider, JwtTokenProvider>();
        #endregion

        #region ScopedServices
        builder.Services.AddScoped<IRepository<Group>, GroupRepository>();

        builder.Services.AddScoped<IGroupRepository, GroupRepository>();
        builder.Services.AddScoped<IExpenseRepository, ExpenseRepository>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<ICurrencyRepository, CurrencyRepository>();

        builder.Services.AddScoped<IExpenseService, ExpenseService>();
        builder.Services.AddScoped<IGroupService, GroupService>();
        builder.Services.AddScoped<IInvitationLinkService, InvitationLinkService>();

        builder.Services.AddScoped<IBalanceService, BalanceService>();
        builder.Services.AddScoped<IMemberService, MemberService>();
        builder.Services.AddScoped<ISettlementService, SettlementService>();
        builder.Services.AddScoped<IGroupSummaryService, GroupSummaryService>();
        builder.Services.AddScoped<INotificationService, NotificationService>();

        builder.Services.AddScoped<IAuthUser, AuthUser>();
        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IUserChecker, UserChecker>();

        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        //builder.Services.AddScoped<IServiceBus, ServiceBus>();

        #endregion

        #region Options

        builder.Services.Configure<LinkInvitation>(builder.Configuration.GetSection("LinkInvitation"));
        builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("Jwt"));

        #endregion

        #region MassTransit

        //builder.Services.AddMassTransit(x =>
        //{
        //    x.AddConsumer<ProductAddedEventConsumer>();
        //    x.UsingRabbitMq((context, cfg) =>
        //    {

        //        cfg.Host("localhost", "/", h => {
        //            h.Username("kalo");
        //            h.Password("kalo");
        //        });

        //        //cfg.ConfigureEndpoints(context);
        //        cfg.ReceiveEndpoint("TestQueue",
        //            e => { e.ConfigureConsumer<ProductAddedEventConsumer>(context); });
        //    });
        //});

        #endregion

        #region MappingInit
        builder.Services.AddMapster();

        MappingConfig.Init();
        #endregion

        #region SignalR
        builder.Services.AddSignalR();
        #endregion

        builder.Services.AddControllers();

        #region Swagger
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Enter: Bearer {your JWT token}"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });
        #endregion

        #region Serilog
        builder.Host.UseSerilog((ctx, services, cfg) => cfg
            .MinimumLevel.Debug()
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.File("logs/log-.txt")
            .WriteTo.Elasticsearch([new Uri("http://localhost:9200")], opts =>
            {
                opts.DataStream = new DataStreamName("logs", "iou1", "api");
                opts.BootstrapMethod = BootstrapMethod.Failure;
                opts.ConfigureChannel = channelOpts =>
                {
                    channelOpts.BufferOptions = new BufferOptions
                    {

                    };
                };

                opts.TextFormatting = new EcsTextFormatterConfiguration<LogEventEcsDocument>
                {
                    IncludeHost = false,
                    IncludeProcess = false,
                    IncludeUser = false,
                    IncludeActivityData = false
                };

            }));
        #endregion

        builder.Services.AddScoped<UserSessionMiddleware>();
        builder.Services.AddAuthorization();
        builder.Services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(o =>
            {
                o.RequireHttpsMetadata = false;
                o.TokenValidationParameters = new()
                {
                    //Podajemy dane do autoryzacji żeby klasa wiedziała jak odczytać/odkodowac token oraz sprawdzić jego poprawność
                    //TODO: Co jak zmienimu Issuer albo Audience?
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]!)),
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    ClockSkew = TimeSpan.Zero,
                    ValidateLifetime = true
                };

                // Configure the Authority to the expected value for
                // the authentication provider. This ensures the token
                // is appropriately validated.
                //o.Authority = "Authority URL"; // TODO: Update URL

                // We have to hook the OnMessageReceived event in order to
                // allow the JWT authentication handler to read the access
                // token from the query string when a WebSocket or 
                // Server-Sent Events request comes in.

                // Sending the access token in the query string is required when using WebSockets or ServerSentEvents
                // due to a limitation in Browser APIs. We restrict it to only calls to the
                // SignalR hub in this code.
                // See https://docs.microsoft.com/aspnet/core/signalr/security#access-token-logging
                // for more information about security considerations when using
                // the query string to transmit the access token.
                o.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];

                        // If the request is for our hub...
                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) &&
                            (path.StartsWithSegments("/NotificationHub")))
                        {
                            // Read the token out of the query string
                            context.Token = accessToken;
                        }

                        return Task.CompletedTask;
                    }
                };
            });

        //builder.Services.AddOpenApi();

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("DevCors", p => p
                .WithOrigins(
                    "http://localhost:4200",
                    "https://localhost:4200",
                    "http://localhost:5173",
                    "https://localhost:5173"
                )
                .AllowAnyHeader()
                .AllowAnyMethod()
            );
        });

        //builder.Services.AddOpenApi();

        var app = builder.Build();

        app.UseCors("DevCors");

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseCors(builder =>
                builder
                    .WithOrigins("http://localhost:5173")
                    .AllowAnyHeader()
                    .AllowAnyMethod());
        }

        //app.MapOpenApi();
        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseMiddleware<UserSessionMiddleware>();
        app.MapControllers();
        app.MapHub<NotificationHub>("/notificationHub");
        app.Run();
    }
}

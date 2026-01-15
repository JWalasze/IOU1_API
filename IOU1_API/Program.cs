using Application.Features.Groups.DeleteGroup.Response;
using Application.Features.Groups.DeleteGroup.Validator;
using Application.Features.Groups.GetGroups.Query;
using Application.Features.Groups.GetGroups.Request;
using Application.Features.Groups.GetGroups.Response;
using Domain.RepoInterfaces;
using Elastic.Channels;
using Elastic.Ingest.Elasticsearch;
using Elastic.Ingest.Elasticsearch.DataStreams;
using Elastic.Serilog.Sinks;
using FluentValidation;
using IOU1.API.Middlewares;
using IOU1.Application.Features.Auth;
using IOU1.Application.Features.Auth.LogIn;
using IOU1.Application.Features.Auth.LogIn.Models;
using IOU1.Application.Features.Groups.AddGroup.Handler;
using IOU1.Application.Features.Groups.AddGroup.Models.Endpoint;
using IOU1.Application.Features.Groups.AddGroup.Validator;
using IOU1.Application.Features.Groups.DeleteGroup.Handler;
using IOU1.Application.Features.Groups.DeleteGroup.Request;
using IOU1.Application.Features.Groups.GetGroups.Handler;
using IOU1.Application.Features.Groups.GetGroups.Validator;
using IOU1.Application.Features.Invitations.DirectInvitation;
using IOU1.Application.Features.Invitations.DirectInvitation.Models;
using IOU1.Application.Features.Invitations.GenerateInvitationKey.Handler;
using IOU1.Application.Features.Invitations.GenerateInvitationKey.Request;
using IOU1.Application.Features.Invitations.GenerateInvitationKey.Validator;
using IOU1.Application.Features.Invitations.UseInvitationLink;
using IOU1.Application.Features.Invitations.UseInvitationLink.Models;
using IOU1.Application.Features.Users.AddUser;
using IOU1.Application.Features.Users.AddUser.Models.Endpoint;
using IOU1.Application.Features.Users.DeleteUser;
using IOU1.Application.Features.Users.DeleteUser.Models.Endpoint;
using IOU1.Application.Mappings;
using IOU1.Application.Mediator;
using IOU1.Application.Options;
using IOU1.Application.Services.Groups;
using IOU1.Application.Services.Invitations;
using IOU1.Application.Services.Members;
using IOU1.Application.Services.Users;
using IOU1.Application.Services.Users.Checker;
using IOU1.Domain.Entities;
using IOU1.Domain.Models;
using IOU1.Domain.RepoInterfaces;
using IOU1.Domain.Services;
using IOU1.Domain.Services.Crypto;
using IOU1.Domain.UnitOfWork;
using IOU1.Infrastructure.Auth;
using IOU1.Infrastructure.Mediator;
using IOU1.Infrastructure.Queries;
using IOU1.Infrastructure.Repositories;
using IOU1.Infrastructure.UnitOfWork;
using IOU1.Persistance.Context;
using IOU1_API.Services;
using Mapster;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;

namespace IOU1.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<IOU1Context>(opt =>
                opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            #region SingletonServices

            builder.Services.AddScoped<IRequestMediator, RequestMediator>();

            builder.Services.AddSingleton<IValidator<GroupsRequest>, GetGroupsValidator>();
            builder.Services.AddSingleton<IValidator<AddGroupRequest>, AddGroupValidator>();
            builder.Services.AddSingleton<IValidator<DeleteGroupRequest>, DeleteGroupValidator>();
            builder.Services.AddSingleton<IValidator<GenerateInvitationKeyRequest>, GenerateInvitationKeyValidator>();
            builder.Services.AddSingleton<IValidator<LogInRequest>, LogInValidator>();
            builder.Services.AddSingleton<IValidator<AddUserRequest>, AddUserValidator>();
            builder.Services.AddSingleton<IValidator<DeleteUserRequest>, DeleteUserValidator>();

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

            builder.Services.AddScoped<IValidator<UseInvitationLinkRequest>, UseInvitationLinkValidator>();

            builder.Services.AddScoped<xdGroupService>();
            builder.Services.AddScoped<ExpensesService>();
            builder.Services.AddScoped<IGroupService, GroupService>();
            builder.Services.AddScoped<IDirectInvitationCreationService, DirectInvitationCreationService>();
            builder.Services.AddScoped<IInvitationLinkService, InvitationLinkService>();
            builder.Services.AddScoped<IMemberService, MemberService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IUserChecker, UserChecker>();

            builder.Services.AddScoped<IValidator<DirectInvitationCreationRequest>, DirectInvitationCreationValidator>();

            builder.Services.AddScoped<IGetGroupsQuery, GetGroupsQuery>();

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddScoped<IAuthUser, AuthUser>();
            //builder.Services.AddScoped<IServiceBus, ServiceBus>();

            #endregion

            #region TransientServices
            builder.Services.AddTransient<IRequestHandler<GroupsRequest, GroupsResponse>, GroupHandler>();
            builder.Services.AddTransient<IRequestHandler<AddGroupRequest, AddGroupResponse>, AddGroupHandler>();
            builder.Services.AddTransient<IRequestHandler<DirectInvitationCreationRequest, DirectInvitationCreationResponse>, DirectInvitationCreationHandler>();
            builder.Services.AddTransient<IRequestHandler<DeleteGroupRequest, DeleteGroupResponse>, DeleteGroupHandler>();
            builder.Services.AddTransient<IRequestHandler<GenerateInvitationKeyRequest, UseInvitationLinkResponse>, GenerateInvitationKeyHandler>();
            builder.Services.AddTransient<IRequestHandler<UseInvitationLinkRequest, UseInvitationLinkResponse>, UseInvitationLinkHandler>();
            builder.Services.AddTransient<IRequestHandler<LogInRequest, LogInResponse>, LogInHandler>();
            builder.Services.AddTransient<IRequestHandler<AddUserRequest, AddUserResponse>, AddUserHandler>();
            builder.Services.AddTransient<IRequestHandler<DeleteGroupRequest, DeleteGroupResponse>, DeleteGroupHandler>();
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

            builder.Services.AddControllers();
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

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.FromLogContext()
                .WriteTo.Elasticsearch([new Uri("http://localhost:9200")], opts =>
                {
                    opts.DataStream = new DataStreamName("logs", "console-example", "demo");
                    opts.BootstrapMethod = BootstrapMethod.Failure;
                    opts.ConfigureChannel = channelOpts =>
                    {
                        channelOpts.BufferOptions = new BufferOptions
                        {

                        };
                    };
                }, transport =>
                {
                    // transport.Authentication(new BasicAuthentication(username, password));
                    // transport.Authentication(new ApiKey(base64EncodedApiKey));
                })
                .CreateLogger();

            builder.Host.UseSerilog((ctx, services, cfg) => cfg
                .MinimumLevel.Debug()
                .Enrich.FromLogContext()
                .WriteTo.Elasticsearch([new Uri("http://localhost:9200")], opts =>
                {
                    opts.DataStream = new DataStreamName("logs", "console-example", "demo");
                    opts.BootstrapMethod = BootstrapMethod.Failure;
                    opts.ConfigureChannel = channelOpts =>
                    {
                        channelOpts.BufferOptions = new BufferOptions
                        {

                        };
                    };
                }, transport =>
                {
                    // transport.Authentication(new BasicAuthentication(username, password));
                    // transport.Authentication(new ApiKey(base64EncodedApiKey));
                }));

            builder.Services.AddScoped<UserSessionMiddleware>();
            builder.Services.AddAuthorization();
            builder.Services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(o =>
                {
                    o.RequireHttpsMetadata = false;
                    o.TokenValidationParameters = new()
                    {
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]!)),
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        ClockSkew = TimeSpan.Zero
                    };
                });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();

                app.UseCors(builder =>
                builder.WithOrigins("http://localhost:5173")
                       .AllowAnyHeader()
                       .AllowAnyMethod());
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMiddleware<UserSessionMiddleware>();
            app.MapControllers();
            app.Run();
        }
    }
}

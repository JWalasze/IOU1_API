using Application.Features.Groups.DeleteGroup.Response;
using Application.Features.Groups.DeleteGroup.Validator;
using Domain.RepoInterfaces;
using Elastic.Channels;
using Elastic.CommonSchema.Serilog;
using Elastic.Ingest.Elasticsearch;
using Elastic.Ingest.Elasticsearch.DataStreams;
using Elastic.Serilog.Sinks;
using FluentValidation;
using IOU1.API.Middlewares;
using IOU1.Application.Features.Auth;
using IOU1.Application.Features.Auth.LogIn.Handler;
using IOU1.Application.Features.Auth.LogIn.Models;
using IOU1.Application.Features.Auth.LogIn.Validator;
using IOU1.Application.Features.Expenses.AddExpense.Handler;
using IOU1.Application.Features.Expenses.AddExpense.Models;
using IOU1.Application.Features.Expenses.AddExpense.Validator;
using IOU1.Application.Features.Expenses.GetExpenses.Handler;
using IOU1.Application.Features.Expenses.GetExpenses.Models;
using IOU1.Application.Features.Expenses.GetExpenses.Validator;
using IOU1.Application.Features.Groups.AddGroup.Handler;
using IOU1.Application.Features.Groups.AddGroup.Models.Endpoint;
using IOU1.Application.Features.Groups.AddGroup.Validator;
using IOU1.Application.Features.Groups.DeleteGroup.Handler;
using IOU1.Application.Features.Groups.DeleteGroup.Request;
using IOU1.Application.Features.Groups.GetGroup.Handler;
using IOU1.Application.Features.Groups.GetGroup.Models.Request;
using IOU1.Application.Features.Groups.GetGroup.Query;
using IOU1.Application.Features.Groups.GetGroup.Validator;
using IOU1.Application.Features.Groups.GetGroups.Handler;
using IOU1.Application.Features.Groups.GetGroups.Models.Request;
using IOU1.Application.Features.Groups.GetGroups.Query;
using IOU1.Application.Features.Groups.GetGroups.Validator;
using IOU1.Application.Features.Invitations.DirectInvitation;
using IOU1.Application.Features.Invitations.DirectInvitation.Models;
using IOU1.Application.Features.Invitations.GenerateInvitationKey.Handler;
using IOU1.Application.Features.Invitations.GenerateInvitationKey.Request;
using IOU1.Application.Features.Invitations.GenerateInvitationKey.Validator;
using IOU1.Application.Features.Invitations.UseInvitationLink;
using IOU1.Application.Features.Invitations.UseInvitationLink.Models;
using IOU1.Application.Features.Members.AddMember.Handler;
using IOU1.Application.Features.Members.AddMember.Models;
using IOU1.Application.Features.Members.AddMember.Validator;
using IOU1.Application.Features.Users.AddUser;
using IOU1.Application.Features.Users.AddUser.Models.Endpoint;
using IOU1.Application.Features.Users.DeleteUser;
using IOU1.Application.Features.Users.DeleteUser.Models.Endpoint;
using IOU1.Application.Mappings;
using IOU1.Application.Mediator;
using IOU1.Application.Options;
using IOU1.Application.Services.Expenses;
using IOU1.Application.Services.Groups;
using IOU1.Application.Services.Invitations;
using IOU1.Application.Services.Members;
using IOU1.Application.Services.Members.Debts;
using IOU1.Application.Services.Users;
using IOU1.Application.Services.Users.Checker;
using IOU1.Domain.Entities;
using IOU1.Domain.Models.Auth.User;
using IOU1.Domain.RepoInterfaces;
using IOU1.Domain.Services.Crypto;
using IOU1.Domain.Services.Users;
using IOU1.Domain.UnitOfWork;
using IOU1.Infrastructure.Auth;
using IOU1.Infrastructure.Mediator;
using IOU1.Infrastructure.Repositories;
using IOU1.Infrastructure.UnitOfWork;
using IOU1.Persistance.Context;
using Mapster;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
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
            builder.Services.AddSingleton<IPasswordHasher, PasswordHasher>();
            builder.Services.AddSingleton<IPasswordComparer, PasswordComparer>();

            builder.Services.AddSingleton<ITokenProvider, JwtTokenProvider>();
            #endregion

            #region ScopedServices - Validators
            builder.Services.AddScoped<IValidator<GetGroupsRequest>, GetGroupsValidator>();
            builder.Services.AddScoped<IValidator<AddGroupRequest>, AddGroupValidator>();
            builder.Services.AddScoped<IValidator<DeleteGroupRequest>, DeleteGroupValidator>();
            builder.Services.AddScoped<IValidator<GenerateInvitationKeyRequest>, GenerateInvitationKeyValidator>();
            builder.Services.AddScoped<IValidator<LogInRequest>, LogInValidator>();
            builder.Services.AddScoped<IValidator<AddUserRequest>, AddUserValidator>();
            builder.Services.AddScoped<IValidator<DeleteUserRequest>, DeleteUserValidator>();
            builder.Services.AddScoped<IValidator<GetGroupRequest>, GetGroupValidator>();
            builder.Services.AddScoped<IValidator<AddMemberRequest>, AddMemberValidator>();
            builder.Services.AddScoped<IValidator<AddExpenseRequest>, AddExpenseValidator>();
            builder.Services.AddScoped<IValidator<GetExpensesRequest>, GetExpensesValidator>();
            #endregion

            #region ScopedServices
            builder.Services.AddScoped<IRepository<Group>, GroupRepository>();

            builder.Services.AddScoped<IGroupRepository, GroupRepository>();
            builder.Services.AddScoped<IExpenseRepository, ExpenseRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<ICurrencyRepository, CurrencyRepository>();

            builder.Services.AddScoped<IValidator<UseInvitationLinkRequest>, UseInvitationLinkValidator>();

            builder.Services.AddScoped<IExpenseService, ExpenseService>();
            builder.Services.AddScoped<IGroupService, GroupService>();
            builder.Services.AddScoped<IDirectInvitationCreationService, DirectInvitationCreationService>();
            builder.Services.AddScoped<IInvitationLinkService, InvitationLinkService>();

            builder.Services.AddScoped<IMemberService, MemberService>();
            builder.Services.AddScoped<IMemberDebtService, MemberDebtService>();

            builder.Services.AddScoped<IAuthService, AuthService>();

            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IUserChecker, UserChecker>();

            builder.Services.AddScoped<IValidator<DirectInvitationCreationRequest>, DirectInvitationCreationValidator>();

            builder.Services.AddScoped<IGetGroupsQuery, GetGroupsQuery>();
            builder.Services.AddScoped<IGetGroupQuery, GetGroupQuery>();

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddScoped<IAuthUser, AuthUser>();
            //builder.Services.AddScoped<IServiceBus, ServiceBus>();

            builder.Services.AddScoped<IRequestMediator, RequestMediator>();


            #endregion

            #region TransientServices
            builder.Services.AddTransient<IGetGroupsHandler, GetGroupsHandler>();
            builder.Services.AddTransient<IGetGroupHandler, GetGroupHandler>();
            builder.Services.AddTransient<IAddMemberHandler, AddMemberHandler>();
            builder.Services.AddTransient<IAddExpenseHandler, AddExpenseHandler>();
            builder.Services.AddTransient<IGetExpensesHandler, GetExpensesHandler>();
            builder.Services.AddTransient<ILogInHandler, LogInHandler>();

            builder.Services.AddTransient<IRequestHandler<AddGroupRequest, AddGroupResponse>, AddGroupHandler>();
            builder.Services.AddTransient<IRequestHandler<DirectInvitationCreationRequest, DirectInvitationCreationResponse>, DirectInvitationCreationHandler>();
            builder.Services.AddTransient<IRequestHandler<DeleteGroupRequest, DeleteGroupResponse>, DeleteGroupHandler>();
            builder.Services.AddTransient<IRequestHandler<GenerateInvitationKeyRequest, UseInvitationLinkResponse>, GenerateInvitationKeyHandler>();
            builder.Services.AddTransient<IRequestHandler<UseInvitationLinkRequest, UseInvitationLinkResponse>, UseInvitationLinkHandler>();
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
                .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Error)
                .Enrich.FromLogContext()
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
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]!)),
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        ClockSkew = TimeSpan.Zero,
                        ValidateLifetime = true
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
            app.Run();
        }
    }
}

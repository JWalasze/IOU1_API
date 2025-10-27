using Application.Features.Groups.AddGroup.Handler;
using Application.Features.Groups.AddGroup.Request;
using Application.Features.Groups.AddGroup.Response;
using Application.Features.Groups.DeleteGroup.Handler;
using Application.Features.Groups.DeleteGroup.Request;
using Application.Features.Groups.DeleteGroup.Response;
using Application.Features.Groups.DeleteGroup.Validator;
using Application.Features.Groups.GetGroups.Handler;
using Application.Features.Groups.GetGroups.Query;
using Application.Features.Groups.GetGroups.Request;
using Application.Features.Groups.GetGroups.Response;
using Application.Mediator;
using Application.Service;
using Domain.RepoInterfaces;
using Elastic.Channels;
using Elastic.Ingest.Elasticsearch;
using Elastic.Ingest.Elasticsearch.DataStreams;
using Elastic.Serilog.Sinks;
using FluentValidation;
using Infrastructure.Mediator;
using IOU1.Application.Features.Invitations.DirectInvitation;
using IOU1.Application.Features.Invitations.DirectInvitation.Models;
using IOU1.Application.Features.Groups.AddGroup.Validator;
using IOU1.Application.Features.Groups.GetGroups.Validator;
using IOU1.Application.Features.Invitations.GenerateInvitationKey.Handler;
using IOU1.Application.Features.Invitations.GenerateInvitationKey.Request;
using IOU1.Application.Features.Invitations.GenerateInvitationKey.Validator;
using IOU1.Application.Options;
using IOU1.Application.Service;
using IOU1.Domain.Entities;
using IOU1.Domain.RepoInterfaces;
using IOU1.Domain.UnitOfWork;
using IOU1.Infrastructure.Queries;
using IOU1.Infrastructure.Repositories;
using IOU1.Infrastructure.UnitOfWork;
using IOU1.Persistance.Context;
using IOU1_API.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;
using IOU1.Application.Features.Invitations.UseInvitationLink.Models;
using IOU1.Application.Features.Invitations.UseInvitationLink;
using IOU1.Application.Mediator;
using IOU1.Application.Features.Auth;
using IOU1.Application.Features.Auth.LogIn.Models;
using IOU1.Application.Features.Auth.LogIn;
using IOU1.Infrastructure.Auth;
using IOU1.Application.Features.Users;
using IOU1.Domain.Services;

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

            builder.Services.AddSingleton<IRequestMediator, RequestMediator>();

            builder.Services.AddSingleton<IValidator<GroupsRequest>, GetGroupsValidator>();
            builder.Services.AddSingleton<IValidator<AddGroupRequest>, AddGroupValidator>();
            builder.Services.AddSingleton<IValidator<DeleteGroupRequest>, DeleteGroupValidator>();
            builder.Services.AddSingleton<IValidator<GenerateInvitationKeyRequest>, GenerateInvitationKeyValidator>();
            builder.Services.AddSingleton<IValidator<LogInRequest>, LogInValidator>();
            builder.Services.AddSingleton<IValidator<AddUserRequest>, AddUserValidator>();
            builder.Services.AddSingleton<IPasswordHasher, PasswordHasher>(); 

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

            builder.Services.AddScoped<IRequestHandler<GroupsRequest, GroupsResponse>, GroupHandler>();
            builder.Services.AddScoped<IRequestHandler<AddGroupRequest, AddGroupResponse>, AddGroupHandler>();
            builder.Services.AddScoped<IRequestHandler<DirectInvitationCreationRequest, DirectInvitationCreationResponse>, DirectInvitationCreationHandler>();
            builder.Services.AddScoped<IRequestHandler<DeleteGroupRequest, DeleteGroupResponse>, DeleteGroupHandler>();
            builder.Services.AddScoped<IRequestHandler<GenerateInvitationKeyRequest, UseInvitationLinkResponse>, GenerateInvitationKeyHandler>();
            builder.Services.AddScoped<IRequestHandler<UseInvitationLinkRequest, UseInvitationLinkResponse>, UseInvitationLinkHandler>();
            builder.Services.AddScoped<IRequestHandler<LogInRequest, LogInResponse>, LogInHandler>();
            builder.Services.AddScoped<IRequestHandler<AddUserRequest, AddUserResponse>, AddUserHandler>();

            builder.Services.AddScoped<IValidator<DirectInvitationCreationRequest>, DirectInvitationCreationValidator>();

            builder.Services.AddScoped<IGetGroupsQuery, GetGroupsQuery>();

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            #endregion

            #region TransientServices

            #endregion

            #region Options

            builder.Services.Configure<LinkInvitation>(builder.Configuration.GetSection("LinkInvitation"));

            #endregion

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

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

            Log.Logger.Error("TEST TEST TEST");

            
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

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}

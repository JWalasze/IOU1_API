using Application.Features.Groups.AddGroup.Handler;
using Application.Features.Groups.AddGroup.Request;
using Application.Features.Groups.AddGroup.Response;
using Application.Features.Groups.AddGroup.Validator;
using Application.Features.Groups.DeleteGroup.Handler;
using Application.Features.Groups.DeleteGroup.Request;
using Application.Features.Groups.DeleteGroup.Response;
using Application.Features.Groups.DeleteGroup.Validator;
using Application.Features.Groups.GetGroups.Handler;
using Application.Features.Groups.GetGroups.Query;
using Application.Features.Groups.GetGroups.Request;
using Application.Features.Groups.GetGroups.Response;
using Application.Features.Groups.GetGroups.Validator;
using Application.Mediator;
using Application.Service;
using Domain.Entities;
using Domain.RepoInterfaces;
using Domain.UnitOfWork;
using FluentValidation;
using Infrastructure.Context;
using Infrastructure.Mediator;
using Infrastructure.Queries;
using Infrastructure.Repositories;
using Infrastructure.UnitOfWork;
using IOU1.Domain.RepoInterfaces;
using IOU1_API.Services;
using Microsoft.EntityFrameworkCore;

namespace IOU1_API
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

            #endregion

            #region ScopedServices

            builder.Services.AddScoped<IRepository<Group>, GroupRepository>();
            builder.Services.AddScoped<IGroupRepository, GroupRepository>();
            builder.Services.AddScoped<IExpenseRepository, ExpenseRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<ICurrencyRepository, CurrencyRepository>();

            builder.Services.AddScoped<xdGroupService>();
            builder.Services.AddScoped<ExpensesService>();
            builder.Services.AddScoped<IGroupService, GroupService>();

            builder.Services.AddScoped<IRequestHandler<GroupsRequest, GroupsResponse>, GroupHandler>();
            builder.Services.AddScoped<IRequestHandler<AddGroupRequest, AddGroupResponse>, AddGroupHandler>();
            builder.Services.AddScoped<IRequestHandler<DeleteGroupRequest, DeleteGroupResponse>, DeleteGroupHandler>();

            builder.Services.AddScoped<IGetGroupsQuery, GetGroupsQuery>();

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            #endregion

            #region TransientServices

            #endregion

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddLogging();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}

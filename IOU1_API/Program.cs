using Application.Features.AddGroup.Handler;
using Application.Features.AddGroup.Request;
using Application.Features.AddGroup.Response;
using Application.Features.AddGroup.Service;
using Application.Features.AddGroup.Validator;
using Application.Features.Groups.Handler;
using Application.Features.Groups.Query;
using Application.Features.Groups.Request;
using Application.Features.Groups.Response;
using Application.Features.Groups.Validator;
using Application.Mediator;
using Domain.RepoInterfaces;
using Domain.UnitOfWork;
using FluentValidation;
using Infrastructure.Context;
using Infrastructure.Mediator;
using Infrastructure.Queries;
using Infrastructure.Repositories;
using Infrastructure.UnitOfWork;
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

            #endregion

            #region ScopedServices

            builder.Services.AddScoped<IGroupRepository, GroupRepository>();
            builder.Services.AddScoped<xdGroupService>();
            builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
            builder.Services.AddScoped<IExpenseRepository, ExpenseRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<ITransactionStatusRepository, TransactionStatusRepository>();
            builder.Services.AddScoped<ICurrencyRepository, CurrencyRepository>();
            builder.Services.AddScoped<TransactionService>();
            builder.Services.AddScoped<IGroupService, GroupService>();

            builder.Services.AddScoped<IRequestHandler<GroupsRequest, GroupsResponse>, GroupHandler>();
            builder.Services.AddScoped<IRequestHandler<AddGroupRequest, AddGroupResponse>, AddGroupHandler>();
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

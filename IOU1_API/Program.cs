using Application.Features.Groups.Handler;
using Application.Features.Groups.Request;
using Application.Features.Groups.Response;
using Application.Mediator;
using Domain.RepoInterfaces;
using Infrastructure.Context;
using Infrastructure.Mediator;
using Infrastructure.Repositories;
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

            #endregion

            #region ScopedServices

            builder.Services.AddScoped<IGroupRepository, GroupRepository>();
            builder.Services.AddScoped<GroupService>();
            builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<ITransactionStatusRepository, TransactionStatusRepository>();
            builder.Services.AddScoped<ICurrencyRepository, CurrencyRepository>();
            builder.Services.AddScoped<TransactionService>();

            #endregion

            #region TransientServices

            builder.Services.AddTransient<IRequestHandler<GroupsRequest, GroupsResponse>, GroupHandler>();

            #endregion

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

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

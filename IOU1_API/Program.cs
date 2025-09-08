using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace IOU1_API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<IOU1Context>(opt => 
                opt.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionStrings:DefaultConnection")));

            #region SingletonServices
            #endregion

            #region ScopedServices
            #endregion

            #region TransientServices
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

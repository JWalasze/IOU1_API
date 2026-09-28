using IOU1.API.Middlewares;
using IOU1.Api.Pipelines;
using IOU1.Infrastructure.Notifications;

var builder = WebApplication.CreateBuilder(args);

builder.AddDatabasePipeline();
builder.AddScrutorPipeline();
builder.AddSingletonServicesPipeline();
builder.AddScopedServicesPipeline();
builder.AddOptionsPipeline();
builder.AddMiddlewaresPipeline();
builder.AddMassTransitPipeline();
builder.AddOpenApiPipeline();
builder.AddSerilogPipeline();
builder.AddSecurityPipeline();
builder.AddCorsPipeline();
builder.AddWebPipeline();

var app = builder.Build();

app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseCors("DevCors");
app.AddDevPipeline();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<UserSessionMiddleware>();
app.MapControllers();
app.MapHub<NotificationHub>("/notificationHub");

await app.RunAsync();

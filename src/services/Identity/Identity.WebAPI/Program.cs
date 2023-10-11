using Identity.Infrastructure.Extensions;
using Identity.WebAPI.Middleware;
using Identity.WebAPI.Extensions.Swagger;
using Identity.WebAPI.Extensions.Identity;
using Identity.Application;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

builder.Services.ConfigureIdentity();
builder.Services.ConfigureAuthentication(config);
builder.Services.ConfigureAuthorization();
builder.Services.ConfigureIdentityServer(config);
builder.Services.ConfigureDatabaseConnection(config);
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureSwagger(config);
builder.Services.RegisterApplicationDependencies();
builder.Services.AddControllers();

builder.Services.AddLogging(builder =>
{
    builder.AddConsole();
    builder.SetMinimumLevel(LogLevel.Trace);
});

var app = builder.Build();

app.InitializeDatabase();

app.UseMiddleware<ExceptionMiddleware>();

if (!app.Environment.IsProduction())
{
    app.UseSwaggerWithOAuth();
}

app.UseAuthentication();
app.UseAuthorization();
app.UseIdentityServer(); 
app.MapControllers();

app.Run();

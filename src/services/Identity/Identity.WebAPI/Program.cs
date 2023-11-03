using Identity.Infrastructure.Extensions;
using Identity.WebAPI.Extensions.Identity;
using Identity.Application;
using Shared.Extensions;
using Shared.Middleware;
using Identity.Infrastructure.DbContext;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;
var assemblyName = Assembly.GetExecutingAssembly().GetName().Name!;

builder.Services.ConfigureIdentity();
builder.Services.ConfigureAuthentication(config);
builder.Services.ConfigureAuthorization();
builder.Services.ConfigureIdentityServer(config);
builder.Services.ConfigureDatabaseConnection(config);
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureSwagger(config, assemblyName);
builder.Services.RegisterApplicationDependencies();
builder.Services.AddControllers();
builder.Services.ConfigureCors();

var app = builder.Build();

app.InitializeDatabase<AuthDbContext>();

app.UseCors();
app.UseMiddleware<ExceptionMiddleware>();

if (!app.Environment.IsProduction())
{
    app.UseSwaggerWithOAuth(config);
}

app.UseAuthentication();
app.UseAuthorization();
app.UseIdentityServer();
app.MapControllers();

app.Run();

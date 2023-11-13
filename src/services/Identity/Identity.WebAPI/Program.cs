using Identity.Infrastructure.Extensions;
using Identity.WebAPI.Extensions.Identity;
using Identity.Application;
using Shared.Extensions;
using Identity.Infrastructure.DbContext;
using System.Reflection;
using Shared.Middleware;
using Identity.WebAPI.Extensions;
using Shared.SharedModels;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("passwords.json", optional: false, reloadOnChange: true);

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
builder.Services.ConfigureMassTransit(config);
builder.Services.AddUserRequestRepository(config);
builder.Services.AddConfigurationSection<EmailCredentials>(config);

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
app.UseMiddleware<UserCacheMiddleware>();
app.MapControllers();

app.Run();

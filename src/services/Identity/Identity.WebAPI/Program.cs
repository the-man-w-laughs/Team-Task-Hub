using Identity.Infrastructure.Extensions;
using Identity.WebAPI.Middleware;
using Identity.WebAPI.Extensions.Swagger;
using Identity.WebAPI.Extensions.Identity;
using Identity.Application;
using Identity.WebAPI.Extensions;

var builder = WebApplication.CreateBuilder(args);
var env = builder.Environment;
var config = builder.Configuration;

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureSwagger(config);
builder.Services.ConfigureAuthentication();
builder.Services.ConfigureAuthorization();
builder.Services.ConfigureDatabaseConnection(config);
builder.Services.ConfigureIdentity();
builder.Services.ConfigureIdentityServer(config);
builder.Services.ConfigureCors(env);
builder.Services.RegisterApplicationDependencies();

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

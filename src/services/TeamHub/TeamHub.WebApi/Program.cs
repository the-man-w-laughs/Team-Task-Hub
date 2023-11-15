using Shared.Extensions;
using TeamHub.DAL.Extensions;
using TeamHub.BLL.Extensions;
using TeamHub.BLL;
using System.Reflection;
using Shared.Middleware;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;
var assemblyName = Assembly.GetExecutingAssembly().GetName().Name!;

builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureSwagger(config, assemblyName);
builder.Services.AddControllers();
builder.Services.ConfigureCors();
builder.Services.RegisterValidators();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.ConfigureAuthentication(config);
builder.Services.ConfigureAuthorization();
builder.Services.RegisterDLLDependencies(config);
builder.Services.RegisterAutomapperProfiles();
builder.Services.ConfigureMediatR();
builder.Services.ReristerRrpcService();

var app = builder.Build();

app.UseCors();
app.UseMiddleware<ExceptionMiddleware>();
app.UseRouting();
app.UseGrpcService();

if (!app.Environment.IsProduction())
{
    app.UseSwaggerWithOAuth(config);
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

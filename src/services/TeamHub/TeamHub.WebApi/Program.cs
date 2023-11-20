using Shared.Extensions;
using TeamHub.DAL.Extensions;
using TeamHub.BLL.Extensions;
using TeamHub.BLL;
using System.Reflection;
using Shared.Middleware;
using Shared.SharedModels;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("passwords.json", optional: false, reloadOnChange: true);

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
builder.Services.ConfigureMassTransit(config);
builder.Services.AddUserRequestRepository(config);
builder.Services.ReristerRrpcService();
builder.Services.AddConfigurationSection<EmailCredentials>(config);
builder.Services.RegisterServices();
builder.Services.RegisterHangfire(config);

var app = builder.Build();

app.UseCors();

app.UseMiddleware<ExceptionMiddleware>();
app.UseRouting();

if (!app.Environment.IsProduction())
{
    app.UseSwaggerWithOAuth(config);
}

app.UseHangfireWithDashboard(config);
app.UseDailyMailing();

app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<UserCacheMiddleware>();
app.MapControllers();
app.UseGrpcService();

app.Run();

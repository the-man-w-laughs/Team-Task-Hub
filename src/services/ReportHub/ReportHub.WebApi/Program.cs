using System.Reflection;
using Shared.Extensions;
using Shared.Middleware;
using ReportHub.BLL.Extensions;
using ReportHub.DAL.Extensions;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;
var assemblyName = Assembly.GetExecutingAssembly().GetName().Name!;

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.ConfigureSwagger(config, assemblyName);
builder.Services.ConfigureCors();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.ConfigureAuthentication(config);
builder.Services.ConfigureAuthorization();
builder.Services.ConfigureMongoDb(config);
builder.Services.ConfigureServices();
builder.Services.RegisterAutomapperProfiles();

var app = builder.Build();

app.UseCors();
app.UseMiddleware<ExceptionMiddleware>();

if (!app.Environment.IsProduction())
{
    app.UseSwaggerWithOAuth(config);
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

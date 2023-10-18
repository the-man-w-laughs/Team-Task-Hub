using Shared.Extensions;
using TeamHub.DAL.DBContext;
using TeamHub.DAL.Extensions;
using TeamHub.BLL.Extensions;
using TeamHub.WebApi.Middleweres;
using TeamHub.BLL;
using System.Reflection;
using Shared.Extensions;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;
var assemblyName = Assembly.GetExecutingAssembly().GetName().Name!;

builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureSwagger(config, assemblyName);
builder.Services.RegisterDLLDependencies(config);
builder.Services.AddControllers();
builder.Services.ConfigureCors();
builder.Services.RegisterValidators();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.ConfigureMediatR();
builder.Services.RegisterBLLDependencies();
builder.Services.ConfigureAuthentication(config);
builder.Services.ConfigureAuthorization();

var app = builder.Build();

app.InitializeDatabase<TeamHubDbContext>();

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

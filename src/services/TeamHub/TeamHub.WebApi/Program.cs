using Shared.Extensions;
using Shared.Middleware;
using TeamHub.DAL.DBContext;
using TeamHub.DAL.Extensions;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;

builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureSwagger(config);
builder.Services.RegisterDalDependencies(config);
builder.Services.AddControllers();
builder.Services.ConfigureCors();

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

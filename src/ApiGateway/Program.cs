using System.Reflection;
using ApiGateway.Extensions;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Shared.Extensions;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
var assemblyName = Assembly.GetExecutingAssembly().GetName().Name!;

builder.Configuration
    .AddJsonFile(config["Ocelot:JsonFile"], optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();

builder.Services.AddOcelot(builder.Configuration);
builder.Services.AddSwaggerForOcelot(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureLogging(builder, assemblyName);
builder.Services.AddRoutingOptions();
builder.Services.AddCustomControllers();
builder.Services.ConfigureCors(config);

var app = builder.Build();

app.UseCors();
app.UseRouting();

if (!app.Environment.IsProduction())
{
    app.UseOcelotSwagger(config);
}

app.UseWebSockets();

await app.UseOcelot();

app.Run();

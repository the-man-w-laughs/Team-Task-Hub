using ApiGateway.Extensions;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Shared.Extensions;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

builder.Configuration
    .AddJsonFile(config["Ocelot:JsonFile"], optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();

builder.Services.AddOcelot(builder.Configuration);
builder.Services.AddSwaggerForOcelot(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureLogging(builder);

var app = builder.Build();

app.UseRouting();

if (!app.Environment.IsProduction())
{
    app.UseOcelotSwagger(config);
}

app.UseWebSockets();

await app.UseOcelot();

app.Run();

using ApiGateway.Extensions;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

builder.Configuration
    .AddJsonFile(builder.Configuration["Ocelot:JsonFile"], optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();
builder.Services.AddOcelot(builder.Configuration);
builder.Services.AddSwaggerForOcelot(builder.Configuration);

builder.Services.AddControllers();

builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseRouting();

if (!app.Environment.IsProduction())
{
    app.UseOcelotSwagger(config);
}

app.UseWebSockets();
await app.UseOcelot();

app.Run();

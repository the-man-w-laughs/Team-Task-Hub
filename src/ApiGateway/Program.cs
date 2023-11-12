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
builder.Services.AddUserRequestRepository(config);
builder.Services.ConfigureAuthentication(config);

var app = builder.Build();

app.UseRouting();

app.UseSwaggerForOcelotUI(opt =>
{
    opt.PathToSwaggerGenerator = app.Configuration["Ocelot:PathToSwaggerGen"];
});

app.UseAuthentication();

app.UseWebSockets();

var configuration = new OcelotPipelineConfiguration().AddUserCacheMiddleware();
await app.UseOcelot(configuration);

app.Run();

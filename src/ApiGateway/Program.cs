using ApiGateway.Extensions;
using ApiGateway.Middlewares;
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
builder.Services.AddUserRequestRepository(config);

var app = builder.Build();

app.UseMiddleware<UserCacheMiddleware>();

app.UseRouting();

app.UseSwaggerForOcelotUI(opt =>
{
    opt.PathToSwaggerGenerator = app.Configuration["Ocelot:PathToSwaggerGen"];
});

app.UseWebSockets();
await app.UseOcelot();

app.Run();

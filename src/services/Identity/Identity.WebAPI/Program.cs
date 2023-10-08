using Identity.WebAPI.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.ConfigureIdentity();


var app = builder.Build();
app.MapGet("/", () => "Hello World!");
app.UseRouting();
app.UseIdentityServer();

app.Run();

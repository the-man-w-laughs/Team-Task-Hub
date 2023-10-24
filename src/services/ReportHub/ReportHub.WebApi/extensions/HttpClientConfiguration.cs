namespace ReportHub.WebApi.Extensions;

public static class HttpClientConfiguration
{
    public static void ConfigureHttpClient(this IServiceCollection services)
    {
        services.AddHttpClient(
            "TeamHubClient",
            client =>
            {
                client.BaseAddress = new Uri("http://localhost:5052/");
            }
        );

        services.AddHttpClient(
            "NginxClient",
            client =>
            {
                client.BaseAddress = new Uri("http://localhost:5050/");
            }
        );
    }
}

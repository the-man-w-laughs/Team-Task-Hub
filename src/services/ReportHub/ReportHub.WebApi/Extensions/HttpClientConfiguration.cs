namespace ReportHub.WebApi.Extensions;

public static class HttpClientConfiguration
{
    public static void ConfigureHttpClient(this IServiceCollection services, IConfiguration config)
    {
        string endpoint = config["TeamHubConfig:Endpoint"];

        services.AddHttpClient(
            "TeamHubClient",
            client =>
            {
                client.BaseAddress = new Uri(endpoint);
            }
        );
    }
}

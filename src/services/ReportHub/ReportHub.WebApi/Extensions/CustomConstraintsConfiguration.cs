namespace ReportHub.WebApi.Extensions;

public static class CustomConstraintsConfiguration
{
    public static void RegisterCustomConstraint(this IServiceCollection services)
    {
        services.Configure<RouteOptions>(options =>
        {
            options.ConstraintMap["string"] = typeof(string);
        });

        services.AddSingleton<IInlineConstraintResolver, DefaultInlineConstraintResolver>();
    }
}

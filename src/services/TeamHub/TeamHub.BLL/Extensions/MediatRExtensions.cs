using MediatR;
using Microsoft.Extensions.DependencyInjection;
using TeamHub.BLL.MediatR.CQRS.Comment.Commands;
using TeamHub.BLL.MediatR.CQRS.Projects.Commands;
using TeamHub.BLL.MediatR.Pipeline;

namespace TeamHub.BLL.Extensions;

public static class MediatRExtensions
{
    public static void ConfigureMediatR(this IServiceCollection services)
    {
        services
            .AddMediatR(x => x.RegisterServicesFromAssembly(typeof(CreateProjectCommand).Assembly))
            .AddScoped(typeof(IPipelineBehavior<,>), typeof(AuthenticationBehaviour<,>))
            .AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
    }
}

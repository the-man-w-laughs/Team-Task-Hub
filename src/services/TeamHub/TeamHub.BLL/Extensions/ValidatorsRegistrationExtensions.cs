using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using TeamHub.BLL.DtoValidators;

namespace TeamHub.BLL.Extensions
{
    public static class ValidatorsRegistrationExtensions
    {
        public static void RegisterValidators(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<ProjectRequestDtoValidator>();
        }
    }
}

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions;

public static partial class LmsRegistration
{
    public static IServiceCollection AddLmsServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Add application services
        services.AddServices();
        // Add repositories
        services.AddRepositories();
        return services;
    }
}

using LMS.Domain.Repositories;
using LMS.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions;

public static partial class LmsRegistration
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(IGenericRepository<,>), typeof(Repository<,>));
        return services;
    }
}

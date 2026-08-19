using Application.Contracts.Auth;
using Application.Contracts.Users;
using Application.Services.Auth;
using Application.Services.Users;
using LMS.Application.Contracts.UOW;
using LMS.Application.Services.UOW;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions;

public static partial class LmsRegistration
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IUserAppService, UserAppService>();
        services.AddScoped<IAuthenticationAppService, AuthenticationAppService>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        return services;
    }
}

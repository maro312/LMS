using Application.Contracts.Auth;
using Application.Contracts.Lookups;
using Application.Contracts.Users;
using Application.Services.Auth;
using Application.Services.Lookups;
using Application.Services.Users;
using Application.Contracts.Books;
using Application.Services.Books;
using LMS.Application.Contracts.UOW;
using LMS.Application.Services.UOW;
using Microsoft.Extensions.DependencyInjection;
using Application.Services.BorrowingRequests;
using Application.Contracts.BorrowingRequest;

namespace Application.Extensions;

public static partial class LmsRegistration
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<LMS.Infrastructure.Services.ICurrentUserService, LMS.Infrastructure.Services.CurrentUserService>();
        services.AddScoped<IUserAppService, UserAppService>();
        services.AddScoped<IAuthenticationAppService, AuthenticationAppService>();
        services.AddScoped<ICategoryAppService, CategoryAppService>();
        services.AddScoped<IBookAppService, BookAppService>();
        services.AddScoped<IBorrowingAppService, BorrowingAppService>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        return services;
    }
}


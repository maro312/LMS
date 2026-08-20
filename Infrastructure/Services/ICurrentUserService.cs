using System.Security.Claims;

namespace LMS.Infrastructure.Services
{
    public interface ICurrentUserService
    {
        string? UserId { get; }
        string? Email { get; }
        bool IsAuthenticated { get; }
        string? GetClaim(string claimType);
    }
}

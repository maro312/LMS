using Application.Dtos.Auth;
using LMS.Core.Results;

namespace Application.Contracts.Auth;

public interface IAuthenticationAppService
{
    Task<Result<AuthenticationDto>> Register(RegisterationDto dto);
    Task<Result<AuthenticationDto>> Login(LoginDto dto);
    Task<Result<AuthenticationDto>> RefreshTokenAsync(RefreshTokenRequestDto dto);
}


using Application.Contracts.Auth;
using Application.Dtos.Auth;
using LMS.API.Extensions;
using LMS.Core.Results;
using Microsoft.AspNetCore.Mvc;

namespace LMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthenticationAppService _authService;

    public AuthController(IAuthenticationAppService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    [ProducesResponseType(typeof(Result<AuthenticationDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result<AuthenticationDto>), StatusCodes.Status400BadRequest)]
    public async Task<Result<AuthenticationDto>> Register([FromBody] RegisterationDto dto)
    {
        var result = await _authService.Register(dto);
        return result;
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(Result<AuthenticationDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result<AuthenticationDto>), StatusCodes.Status400BadRequest)]
    public async Task<Result<AuthenticationDto>> Login([FromBody] LoginDto dto)
    {
        var result = await _authService.Login(dto);
        return result;
    }

    [HttpPost("refresh-token")]
    [ProducesResponseType(typeof(Result<AuthenticationDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result<AuthenticationDto>), StatusCodes.Status400BadRequest)]
    public async Task<Result<AuthenticationDto>> RefreshToken([FromBody] RefreshTokenRequestDto dto)
    {
        var result = await _authService.RefreshTokenAsync(dto);
        return result;
    }

    [HttpPost("logout")]
    [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status400BadRequest)]
    public async Task<Result<bool>> Logout([FromBody] LogoutDto dto)
    {
        var result = await _authService.LogoutAsync(dto);
        return result;
    }
}

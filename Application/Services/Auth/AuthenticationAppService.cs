using Application.Contracts.Auth;
using Application.Contracts.Users;
using Application.Dtos.Auth;
using Application.Dtos.Users;
using LMS.Core.Results;
using LMS.Domain.Constants;
using LMS.Infrastructure.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Application.Services.Auth;

public class AuthenticationAppService : IAuthenticationAppService
{
    private readonly UserManager<IdentityUser<Guid>> _userManager;
    private readonly IUserAppService _userAppService;
    private readonly JwtOptions _jwtOptions;

    public AuthenticationAppService(
        UserManager<IdentityUser<Guid>> userManager,
        IUserAppService userAppService,
        IOptions<JwtOptions> jwtOptions)
    {
        _userManager = userManager;
        _userAppService = userAppService;
        _jwtOptions = jwtOptions.Value;
    }

    public async Task<Result<AuthenticationDto>> Register(RegisterationDto dto)
    {
        IdentityUser<Guid>? userExist = await _userManager.FindByEmailAsync(dto.Email);
        if (userExist is not null)
        {
            return Result<AuthenticationDto>.BadRequest("User already exists with this email.");
        }

        IdentityUser<Guid> newUser = new IdentityUser<Guid>
        {
            Email = dto.Email,
            UserName = dto.Email,
        };

        var result = await _userManager.CreateAsync(newUser, dto.Password);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return Result<AuthenticationDto>.BadRequest(errors);
        }

        var roleResult = await _userManager.AddToRoleAsync(newUser, UserRoles.User);
        if (!roleResult.Succeeded)
        {
            var roleErrors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
            return Result<AuthenticationDto>.BadRequest(roleErrors);
        }

        CreateUpdateUserDto user = new CreateUpdateUserDto
        {
            FullName = dto.FullName,
            IsActive = true,
            IdentityUserId = newUser.Id
        };

        var userDto = await _userAppService.CreateAsync(user);

        var token = await GenerateJwtToken(newUser);
        return Result<AuthenticationDto>.Success(new AuthenticationDto
        {
            Token = token,
            Email = newUser.Email
        });
    }

    public async Task<Result<AuthenticationDto>> Login(LoginDto dto)
    {
        if (dto is null || string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
        {
            return Result<AuthenticationDto>.BadRequest("Email and Password are required.");
        }

        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user is null)
        {
            return Result<AuthenticationDto>.BadRequest("Invalid email or password.");
        }

        var isPasswordValid = await _userManager.CheckPasswordAsync(user, dto.Password);
        if (!isPasswordValid)
        {
            return Result<AuthenticationDto>.BadRequest("Invalid email or password.");
        }

        var token = await GenerateJwtToken(user);
        return Result<AuthenticationDto>.Success(new AuthenticationDto
        {
            Token = token,
            Email = user.Email ?? string.Empty
        });
    }

    private async Task<string> GenerateJwtToken(IdentityUser<Guid> user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = System.Text.Encoding.ASCII.GetBytes(_jwtOptions.Secret);

        var claims = new List<Claim>
        {
            new Claim("Id", user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Sub, user.Email ?? string.Empty),
            new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var roles = await _userManager.GetRolesAsync(user);
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(15),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}



using Application.Contracts.Auth;
using Application.Contracts.Users;
using Application.Dtos.Auth;
using Application.Dtos.Users;
using LMS.Application.Contracts.UOW;
using LMS.Core.Results;
using LMS.Domain.Constants;
using LMS.Domain.Entities;
using LMS.Domain.Repositories;
using LMS.Infrastructure.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Application.Services.Auth;

public class AuthenticationAppService : IAuthenticationAppService
{
    private readonly UserManager<IdentityUser<Guid>> _userManager;
    private readonly IUserAppService _userAppService;
    private readonly JwtOptions _jwtOptions;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<AuthenticationAppService> _logger;
    private readonly IGenericRepository<RefreshToken, Guid> _refreshTokenRepository;

    public AuthenticationAppService(
        UserManager<IdentityUser<Guid>> userManager,
        IUserAppService userAppService,
        IOptions<JwtOptions> jwtOptions,
        IUnitOfWork unitOfWork,
        ILogger<AuthenticationAppService> logger,
        IGenericRepository<RefreshToken, Guid> refreshTokenRepository)
    {
        _userManager = userManager;
        _userAppService = userAppService;
        _jwtOptions = jwtOptions.Value;
        _unitOfWork = unitOfWork;
        _logger = logger;
        _refreshTokenRepository = refreshTokenRepository;
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

        await _unitOfWork.BeginTransactionAsync();

        try
        {
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

            var jwtData = await GenerateJwtToken(newUser);
            var refreshToken = GenerateRefreshToken(newUser.Id, jwtData.Jti);
            
            await _refreshTokenRepository.AddAsync(refreshToken);
            await _unitOfWork.CommitTransactionAsync();
            
            return Result<AuthenticationDto>.Success(new AuthenticationDto
            {
                Token = jwtData.Token,
                Email = newUser.Email,
                RefreshToken = refreshToken.Token,
                RefreshTokenExpiration = refreshToken.ExpiryDate
            });
        }
        catch (Exception ex) 
        {
            _logger.LogError(ex, "Error occurred during user registration.");
            await _unitOfWork.RollbackTransactionAsync();
            return Result<AuthenticationDto>.InternalServerError("An error occurred during registration. Please try again.");
        }
        finally
        {
            _unitOfWork.Dispose();
        }
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

        await _unitOfWork.BeginTransactionAsync();
        try
        {
            var jwtData = await GenerateJwtToken(user);
            var refreshToken = GenerateRefreshToken(user.Id, jwtData.Jti);

            await _refreshTokenRepository.AddAsync(refreshToken);
            await _unitOfWork.CommitTransactionAsync();

            return Result<AuthenticationDto>.Success(new AuthenticationDto
            {
                Token = jwtData.Token,
                Email = user.Email ?? string.Empty,
                RefreshToken = refreshToken.Token,
                RefreshTokenExpiration = refreshToken.ExpiryDate
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred during login.");
            await _unitOfWork.RollbackTransactionAsync();
            return Result<AuthenticationDto>.InternalServerError("An error occurred during login.");
        }
        finally
        {
            _unitOfWork.Dispose();
        }
    }

    public async Task<Result<AuthenticationDto>> RefreshTokenAsync(RefreshTokenRequestDto dto)
    {
        var validatedToken = GetPrincipalFromExpiredToken(dto.Token);
        if (validatedToken == null)
        {
            return Result<AuthenticationDto>.BadRequest("Invalid token.");
        }

        var expiryDateUnix = long.Parse(validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);
        var expiryDateTimeUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(expiryDateUnix);

        if (expiryDateTimeUtc > DateTime.UtcNow)
        {
            return Result<AuthenticationDto>.BadRequest("This token hasn't expired yet.");
        }

        var jti = validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
        
        var storedRefreshToken = await _refreshTokenRepository.GetAllQuerable()
            .FirstOrDefaultAsync(x => x.Token == dto.RefreshToken);

        if (storedRefreshToken == null)
        {
            return Result<AuthenticationDto>.BadRequest("This refresh token does not exist.");
        }

        if (DateTime.UtcNow > storedRefreshToken.ExpiryDate)
        {
            return Result<AuthenticationDto>.BadRequest("This refresh token has expired.");
        }

        if (storedRefreshToken.IsRevoked)
        {
            return Result<AuthenticationDto>.BadRequest("This refresh token has been revoked.");
        }

        if (storedRefreshToken.IsUsed)
        {
            // REPLAY PROTECTION: A used token was presented. This means it was potentially stolen.
            // Revoke all tokens for this user using a bulk database update for maximum performance.
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _refreshTokenRepository.GetAllQuerable()
                    .Where(x => x.IdentityUserId == storedRefreshToken.IdentityUserId && !x.IsRevoked)
                    .ExecuteUpdateAsync(s => s.SetProperty(t => t.IsRevoked, true));
                
                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
            }
            finally
            {
                _unitOfWork.Dispose();
            }

            return Result<AuthenticationDto>.BadRequest("This token has already been used. For security reasons, all active sessions have been terminated.");
        }

        if (storedRefreshToken.JwtId != jti)
        {
            return Result<AuthenticationDto>.BadRequest("This refresh token does not match this JWT.");
        }

        // Token is valid. Mark as used and issue a new one (Rotation)
        storedRefreshToken.IsUsed = true;
        
        var user = await _userManager.FindByIdAsync(storedRefreshToken.IdentityUserId.ToString());
        if (user == null)
        {
             return Result<AuthenticationDto>.BadRequest("User not found.");
        }

        await _unitOfWork.BeginTransactionAsync();
        try
        {
            await _refreshTokenRepository.UpdateAsync(storedRefreshToken);

            var newJwtData = await GenerateJwtToken(user);
            var newRefreshToken = GenerateRefreshToken(user.Id, newJwtData.Jti);

            await _refreshTokenRepository.AddAsync(newRefreshToken);
            await _unitOfWork.CommitTransactionAsync();

            return Result<AuthenticationDto>.Success(new AuthenticationDto
            {
                Token = newJwtData.Token,
                Email = user.Email ?? string.Empty,
                RefreshToken = newRefreshToken.Token,
                RefreshTokenExpiration = newRefreshToken.ExpiryDate
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error refreshing token.");
            await _unitOfWork.RollbackTransactionAsync();
            return Result<AuthenticationDto>.InternalServerError("An error occurred. Please try again.");
        }
        finally
        {
            _unitOfWork.Dispose();
        }
    }

    /// <summary>
    /// Validates the expired JWT token and returns the ClaimsPrincipal if valid.
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    private ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(_jwtOptions.Secret)),
            ValidateLifetime = false // Here we are saying that we don't care about the token's expiration date
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
        var jwtSecurityToken = securityToken as JwtSecurityToken;
        if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
        {
            return null;
        }

        return principal;
    }

    private RefreshToken GenerateRefreshToken(Guid userId, string jwtId)
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);

        return new RefreshToken
        {
            Token = Convert.ToBase64String(randomNumber),
            JwtId = jwtId,
            IsUsed = false,
            IsRevoked = false,
            IdentityUserId = userId,
            AddedDate = DateTime.UtcNow,
            ExpiryDate = DateTime.UtcNow.AddDays(7) // Refresh token valid for 7 days
        };
    }

    private async Task<(string Token, string Jti)> GenerateJwtToken(IdentityUser<Guid> user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = System.Text.Encoding.ASCII.GetBytes(_jwtOptions.Secret);
        var jti = Guid.NewGuid().ToString();

        var claims = new List<Claim>
        {
            new Claim("Id", user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Sub, user.Email ?? string.Empty),
            new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
            new Claim(JwtRegisteredClaimNames.Jti, jti)
        };

        var roles = await _userManager.GetRolesAsync(user);
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return (tokenHandler.WriteToken(token), jti);
    }
}



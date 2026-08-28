using System;
using LMS.Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace LMS.Domain.Entities;

/// <summary>
/// Represents a refresh token used for authenticating API requests after a JWT expires.
/// </summary>
public class RefreshToken : BaseEntity<Guid>
{
    /// <summary>
    /// The randomly generated refresh token string.
    /// </summary>
    public string Token { get; set; } = string.Empty;

    /// <summary>
    /// The ID of the JWT this refresh token is paired with.
    /// </summary>
    public string JwtId { get; set; } = string.Empty;

    /// <summary>
    /// Indicates if the refresh token has already been used to issue a new token.
    /// </summary>
    public bool IsUsed { get; set; }

    /// <summary>
    /// Indicates if the refresh token has been explicitly revoked (e.g., due to theft or manual logout).
    /// </summary>
    public bool IsRevoked { get; set; }

    /// <summary>
    /// The date and time when this refresh token was issued.
    /// </summary>
    public DateTime AddedDate { get; set; }

    /// <summary>
    /// The date and time when this refresh token expires.
    /// </summary>
    public DateTime ExpiryDate { get; set; }

    /// <summary>
    /// Foreign key referencing the associated ASP.NET Core Identity user.
    /// </summary>
    public Guid IdentityUserId { get; set; }

    /// <summary>
    /// Navigation property to the associated IdentityUser.
    /// </summary>
    public IdentityUser<Guid>? IdentityUser { get; set; }
}

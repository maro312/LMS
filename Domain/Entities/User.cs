using System;
using LMS.Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace LMS.Domain.Entities;

/// <summary>
/// Represents a User entity.
/// </summary>
public class User : AuditableEntity<Guid>
{
    /// <summary>
    /// Foreign key referencing the associated ASP.NET Core Identity user.
    /// </summary>
    public string IdentityUserId { get; set; } = string.Empty;

    /// <summary>
    /// Navigation property to the associated IdentityUser.
    /// </summary>
    public IdentityUser? IdentityUser { get; set; }

    /// <summary>
    /// Display name
    /// </summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Access status
    /// </summary>
    public bool IsActive { get; set; }
}


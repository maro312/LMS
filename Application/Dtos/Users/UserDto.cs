using LMS.Core.Dtos;
using Microsoft.AspNetCore.Identity;

namespace Application.Dtos.Users;

public class UserDto : AuditableDto<Guid>
{
    /// <summary>
    /// Foreign key referencing the associated ASP.NET Core Identity user.
    /// </summary>
    public Guid IdentityUserId { get; set; }

    /// <summary>
    /// Navigation property to the associated IdentityUser.
    /// </summary>
    public IdentityUser<Guid>? IdentityUser { get; set; }

    /// <summary>
    /// Display name
    /// </summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Access status
    /// </summary>
    public bool IsActive { get; set; }
}

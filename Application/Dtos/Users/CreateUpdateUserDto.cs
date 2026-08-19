using System;

namespace Application.Dtos.Users;

public class CreateUpdateUserDto
{
    /// <summary>
    /// Display name
    /// </summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Access status
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Foreign key referencing the associated ASP.NET Core Identity user.
    /// </summary>
    public Guid IdentityUserId { get; set; }
}

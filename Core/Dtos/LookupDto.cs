using LMS.Core.Contracts;

namespace LMS.Core.Dtos;

/// <summary>
/// Base class for Lookup DTOs.
/// </summary>
/// <typeparam name="T">The type of the primary key.</typeparam>
public class LookupDto<T> : AuditableDto<T>
{
    /// <summary>
    /// Display name of the lookup entry.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Optional system/code identifier.
    /// </summary>
    public string? Code { get; set; }

    /// <summary>
    /// Optional description.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Indicates whether the lookup entry is active.
    /// </summary>
    public bool IsActive { get; set; } = true;
}

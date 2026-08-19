using System;
using LMS.Core.Entities;
using LMS.Domain.Lookups;

namespace LMS.Domain.Entities;

/// <summary>
/// Represents a Book entity.
/// </summary>
public class Book : AuditableEntity<Guid>
{
    /// <summary>
    /// Optional ISBN
    /// </summary>
    public string? Isbn { get; set; }

    /// <summary>
    /// Book title
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Primary author name
    /// </summary>
    public string Author { get; set; } = string.Empty;

    /// <summary>
    /// Category lookup reference
    /// </summary>
    public Guid? CategoryId { get; set; }
    public Category? Category { get; set; }

    /// <summary>
    /// Availability flag
    /// </summary>
    public bool IsAvailable { get; set; }

    /// <summary>
    /// Total library copies
    /// </summary>
    public int TotalCopies { get; set; }

    /// <summary>
    /// Copies currently available
    /// </summary>
    public int AvailableCopies { get; set; }
}

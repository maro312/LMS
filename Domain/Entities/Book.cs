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
    /// <summary>
    /// The URL of the book's photo. This property is optional and can be null if no photo is available.
    /// </summary>
    public string? BookPhotoUrl { get; set; } = default!;
    /// <summary>
    /// The name of the publisher. This property is optional and can be null if the publisher's name is not available.
    /// </summary>
    public string? PublisherName { get; set; } = string.Empty;
    /// <summary>
    /// The date when the book was published. This property is optional and can be null if the publish date is not available.
    /// </summary>
    public DateOnly? PublishDate { get; set; }
}

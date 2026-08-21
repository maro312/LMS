using System;
using LMS.Core.Entities;
using LMS.Domain.Enums;

namespace LMS.Domain.Entities;

/// <summary>
/// Represents a Borrowing Request entity.
/// </summary>
public class BorrowingRequest : AuditableEntity<Guid>
{
    /// <summary>
    /// Related book
    /// </summary>
    public Guid BookId { get; set; }
    public Book Book { get; set; }

    /// <summary>
    /// Requesting user
    /// </summary>
    public Guid UserId { get; set; }
    public User Requester { get; set; }

    /// <summary>
    /// Pending, Approved, Denied, Returned, Expired
    /// </summary>
    public BorrowingRequestStatus Status { get; set; }

    /// <summary>
    /// Requested period in days
    /// </summary>
    public int BorrowingPeriodDays { get; set; }

    /// <summary>
    /// Request submission time
    /// </summary>
    public DateTime RequestedAt { get; set; }

    /// <summary>
    /// Approval/denial time
    /// </summary>
    public DateTime? ReviewedAt { get; set; }

    /// <summary>
    /// Admin reviewer ID
    /// </summary>
    public Guid? ReviewedBy { get; set; }
    public User? Reviewer { get; set; }

    /// <summary>
    /// Present when denied
    /// </summary>
    public string? DenyReason { get; set; }
}

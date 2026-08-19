using System;
using LMS.Core.Entities;
using LMS.Domain.Enums;

namespace LMS.Domain.Entities;

/// <summary>
/// Represents a Notification entity.
/// </summary>
public class Notification : AuditableEntity<Guid>
{
    /// <summary>
    /// Receiver of the notification
    /// </summary>
    public Guid RecipientUserId { get; set; }
    public User RecipientUser { get; set; }

    /// <summary>
    /// User or Admin
    /// </summary>
    public string RecipientRole { get; set; } = string.Empty;

    /// <summary>
    /// BorrowRequestCreated, BorrowDueReminder, RequestApproved, RequestDenied
    /// </summary>
    public NotificationType Type { get; set; }

    /// <summary>
    /// Notification title
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Notification message
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Read/unread state
    /// </summary>
    public bool IsRead { get; set; }

}

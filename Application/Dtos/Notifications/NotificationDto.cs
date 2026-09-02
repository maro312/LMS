using System;
using LMS.Domain.Enums;

namespace Application.Dtos.Notifications;

public class NotificationDto
{
    public Guid Id { get; set; }
    public Guid RecipientUserId { get; set; }
    public string RecipientRole { get; set; } = string.Empty;
    public NotificationType Type { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public bool IsRead { get; set; }
    public DateTime? CreatedDate { get; set; }
}

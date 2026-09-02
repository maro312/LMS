using Application.Dtos.Notifications;
using LMS.Domain.Entities;

namespace Application.Mappings;

public static class NotificationMapping
{
    public static NotificationDto ToDto(this Notification entity)
    {
        if (entity == null) return null!;

        return new NotificationDto
        {
            Id = entity.Id,
            RecipientUserId = entity.RecipientUserId,
            RecipientRole = entity.RecipientRole,
            Type = entity.Type,
            Title = entity.Title,
            Message = entity.Message,
            IsRead = entity.IsRead,
            CreatedDate = entity.CreatedDate
        };
    }
}

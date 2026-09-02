using Application.Dtos.Notifications;
using LMS.Core.Results;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Contracts.Notifications;

public interface INotificationAppService
{
    Task<Result<List<NotificationDto>>> GetMyNotificationsAsync();
    Task<Result<bool>> MarkAsReadAsync(Guid notificationId);
    Task<Result<NotificationDto>> CreateNotificationAsync(NotificationDto dto);
}

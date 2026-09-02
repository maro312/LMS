using Application.Contracts.Notifications;
using Application.Dtos.Notifications;
using LMS.Core.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Hubs;

[Authorize]
public sealed class BorrowingHub : Hub
{
    private readonly INotificationAppService _notificationAppService;

    public BorrowingHub(INotificationAppService notificationAppService)
    {
        _notificationAppService = notificationAppService;
    }

    public async Task<Result<List<NotificationDto>>> GetMyNotifications()
    {
        return await _notificationAppService.GetMyNotificationsAsync();
    }

    public async Task<Result<bool>> MarkAsRead(Guid notificationId)
    {
        return await _notificationAppService.MarkAsReadAsync(notificationId);
    }
}

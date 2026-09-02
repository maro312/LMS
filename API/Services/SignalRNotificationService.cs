using API.Hubs;
using Application.Contracts.Notifications;
using Application.Dtos.BorrowingRequest;
using Application.Dtos.Notifications;
using LMS.Domain.Enums;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace API.Services;

public class SignalRNotificationService : INotificationService
{
    private readonly IHubContext<BorrowingHub> _hubContext;
    private readonly INotificationAppService _notificationAppService;

    public SignalRNotificationService(IHubContext<BorrowingHub> hubContext, INotificationAppService notificationAppService)
    {
        _hubContext = hubContext;
        _notificationAppService = notificationAppService;
    }

    public async Task NotifyAdminNewBorrowRequestAsync(BorrwingRequestsDto request)
    {
        // Broadcasts to a specific group or all admins.
        await _hubContext.Clients.Group("Admins").SendAsync("ReceiveNewBorrowRequest", request);
    }

    public async Task NotifyUserBorrowRequestReviewedAsync(Guid identityUserId, Guid domainUserId, string status, string? message = null)
    {
        var isApproved = status.Equals("Approved", StringComparison.OrdinalIgnoreCase);
        
        var dto = new NotificationDto
        {
            RecipientUserId = domainUserId,
            RecipientRole = "User", // Or dynamically derived
            Type = isApproved ? NotificationType.RequestApproved : NotificationType.RequestDenied,
            Title = $"Borrow Request {status}",
            Message = message ?? $"Your borrowing request has been {status.ToLower()}.",
        };

        var result = await _notificationAppService.CreateNotificationAsync(dto);

        if (result.IsSuccess && result.Value != null)
        {
            // Send to the Angular frontend listener 'ReceiveNewNotification'
            await _hubContext.Clients.User(identityUserId.ToString()).SendAsync("ReceiveNewNotification", result.Value);
        }
        
        // Keep the old event for backwards compatibility if needed
        await _hubContext.Clients.User(identityUserId.ToString()).SendAsync("BorrowingRequestReviewed", new 
        {
            Status = status,
            Message = dto.Message
        });
    }
}

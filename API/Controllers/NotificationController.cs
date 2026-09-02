using Application.Contracts.Notifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NotificationController : ControllerBase
{
    private readonly INotificationAppService _notificationAppService;

    public NotificationController(INotificationAppService notificationAppService)
    {
        _notificationAppService = notificationAppService;
    }

    [HttpGet("my-notifications")]
    [Authorize]
    public async Task<IActionResult> GetMyNotifications()
    {
        var result = await _notificationAppService.GetMyNotificationsAsync();
        if (result.IsSuccess)
            return Ok(result);
            
        return BadRequest(result);
    }

    [HttpPatch("{id}/read")]
    [Authorize]
    public async Task<IActionResult> MarkAsRead(Guid id)
    {
        var result = await _notificationAppService.MarkAsReadAsync(id);
        if (result.IsSuccess)
            return Ok(result);
            
        return BadRequest(result);
    }
}

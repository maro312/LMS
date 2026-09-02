using Application.Contracts.Notifications;
using Application.Contracts.Users;
using Application.Dtos.Notifications;
using Application.Mappings;
using LMS.Core.Results;
using LMS.Domain.Entities;
using LMS.Domain.Repositories;
using LMS.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services.Notifications;

public class NotificationAppService : INotificationAppService
{
    private readonly IGenericRepository<Notification, Guid> _repository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUserAppService _userAppService;

    public NotificationAppService(
        IGenericRepository<Notification, Guid> repository,
        ICurrentUserService currentUserService,
        IUserAppService userAppService)
    {
        _repository = repository;
        _currentUserService = currentUserService;
        _userAppService = userAppService;
    }

    public async Task<Result<List<NotificationDto>>> GetMyNotificationsAsync()
    {
        try
        {
            if (Guid.TryParse(_currentUserService.UserId, out var identityUserId))
            {
                var userDto = await _userAppService.GetByIdentityIdAsync(identityUserId);
                if (userDto == null)
                {
                    return Result<List<NotificationDto>>.NotFound("User not found.");
                }

                var userId = userDto.Id;
                var query = _repository.GetAllQuerable();
                
                var notifications = await query
                    .Where(n => n.RecipientUserId == userId)
                    .OrderByDescending(n => n.CreatedDate)
                    .ToListAsync();

                var dtos = notifications.Select(n => n.ToDto()).ToList();
                return Result<List<NotificationDto>>.Success(dtos);
            }
            
            return Result<List<NotificationDto>>.BadRequest("Invalid user ID.");
        }
        catch (Exception ex)
        {
            return Result<List<NotificationDto>>.BadRequest(ex.Message);
        }
    }

    public async Task<Result<bool>> MarkAsReadAsync(Guid notificationId)
    {
        try
        {
            var notification = await _repository.GetByIdAsync(notificationId);
            if (notification == null)
            {
                return Result<bool>.NotFound("Notification not found.");
            }

            // Optional: Verify that the current user owns this notification
            if (Guid.TryParse(_currentUserService.UserId, out var identityUserId))
            {
                var userDto = await _userAppService.GetByIdentityIdAsync(identityUserId);
                if (userDto == null || notification.RecipientUserId != userDto.Id)
                {
                    return Result<bool>.Unauthorized();
                }
            }

            notification.IsRead = true;
            await _repository.UpdateAsync(notification);
            
            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.BadRequest(ex.Message);
        }
    }

    public async Task<Result<NotificationDto>> CreateNotificationAsync(NotificationDto dto)
    {
        try
        {
            var notification = new Notification
            {
                RecipientUserId = dto.RecipientUserId,
                RecipientRole = dto.RecipientRole ?? "User",
                Type = dto.Type,
                Title = dto.Title ?? string.Empty,
                Message = dto.Message ?? string.Empty,
                IsRead = false,
                CreatedDate = DateTime.UtcNow,
                CreatedBy = _currentUserService.UserId ?? "System"
            };

            await _repository.AddAsync(notification);
            await _repository.SaveChangesAsync();

            return Result<NotificationDto>.Success(notification.ToDto());
        }
        catch (Exception ex)
        {
            return Result<NotificationDto>.BadRequest(ex.Message);
        }
    }
}

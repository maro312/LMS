using Application.Dtos.BorrowingRequest;

namespace Application.Contracts.Notifications;

public interface INotificationService
{
    Task NotifyAdminNewBorrowRequestAsync(BorrwingRequestsDto request);
    Task NotifyUserBorrowRequestReviewedAsync(Guid identityUserId, Guid domainUserId, string status, string? message = null);
}

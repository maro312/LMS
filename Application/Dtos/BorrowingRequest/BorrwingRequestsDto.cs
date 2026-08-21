using Application.Dtos.Books;
using Application.Dtos.Users;
using LMS.Core.Dtos;
using LMS.Domain.Entities;
using LMS.Domain.Enums;

namespace Application.Dtos.BorrowingRequest;

public class BorrwingRequestsDto : AuditableDto<Guid>
{
    /// <summary>
    /// Related book
    /// </summary>
    public Guid BookId { get; set; }
    public BookDto Book { get; set; }

    /// <summary>
    /// Requesting user
    /// </summary>
    public Guid UserId { get; set; }
    public UserDto Requester { get; set; }

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
    public UserDto? Reviewer { get; set; }

    /// <summary>
    /// Present when denied
    /// </summary>
    public string? DenyReason { get; set; }
}

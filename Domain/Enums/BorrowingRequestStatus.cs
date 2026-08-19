namespace LMS.Domain.Enums;

/// <summary>
/// Status lifecycle of borrowing requests: Pending, Approved, Denied, Returned, Expired
/// </summary>
public enum BorrowingRequestStatus
{
    Pending,
    Approved,
    Denied,
    Returned,
    Expired
}

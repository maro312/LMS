using LMS.Domain.Enums;

namespace Application.Dtos.BorrowingRequest;

public class ReviewRequest
{
    public BorrowingRequestStatus Status { get; set; }
    public string? DenyReason { get; set; }
}

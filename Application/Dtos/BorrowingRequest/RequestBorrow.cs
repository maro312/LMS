namespace Application.Dtos.BorrowingRequest;

public class RequestBorrow
{
    /// <summary>
    /// Related book
    /// </summary>
    public Guid BookId { get; set; }

    /// <summary>
    /// Requested period in days
    /// </summary>
    public int BorrowingPeriodDays { get; set; }

    /// <summary>
    /// Request submission time
    /// </summary>
    public DateTime RequestedAt { get; set; }
}

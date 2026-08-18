using LMS.Core.Contracts;

namespace LMS.Core.Entities;

public class AuditableEntity<T> : BaseEntity<T>, IAuditableEntity<T>
{
    public string? CreatedBy { get; set; }
    public string? ModifiedBy { get; set; }
    public DateTime? CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
}

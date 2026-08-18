using LMS.Core.Contracts;

namespace LMS.Core.Dtos;

public class AuditableDto<T> : BaseDto<T>, IAuditableEntity<T>
{
    public string? CreatedBy { get; set; }
    public string? ModifiedBy { get; set; }
    public DateTime? CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
}

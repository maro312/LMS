namespace LMS.Core.Contracts;

public interface IAuditableEntity<T> : IEntity<T>
{
    string? CreatedBy { get; set; }
    string? ModifiedBy { get; set; }
    DateTime? CreatedDate { get; set; }
    DateTime? UpdatedDate { get; set; }
}

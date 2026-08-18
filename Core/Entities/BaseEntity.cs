using LMS.Core.Contracts;

namespace LMS.Core.Entities;

public class BaseEntity<T> : IEntity<T>
{
    public T Id { get; set; } = default!;
}

using LMS.Core.Contracts;

namespace LMS.Core.Dtos;

public class BaseDto<T> : IEntity<T>
{
    public T Id { get; set; } = default!;
}

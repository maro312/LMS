namespace LMS.Core.Contracts;

public interface IEntity<TId>
{
    TId Id { get; set; }
}

public interface IEntity<TKey1, TKey2>
{
}

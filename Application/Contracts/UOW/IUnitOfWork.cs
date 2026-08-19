namespace LMS.Application.Contracts.UOW;

public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// Begins a new transaction.
    /// </summary>
    Task BeginTransactionAsync();

    /// <summary>
    /// Commits the current transaction.
    /// </summary>
    Task CommitTransactionAsync();

    /// <summary>
    /// Rolls back the current transaction.
    /// </summary>
    Task RollbackTransactionAsync();
}

namespace Core.Contracts.Cruds;

/// <summary>
/// Interface for getting a list of entities asynchronously.
/// </summary>
/// <typeparam name="TOutputList">The output list type, which must be a class.</typeparam>
public interface IGetListAppService<TOutputList>
    where TOutputList : class
{
    /// <summary>
    /// Retrieves a list of all entities asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation, containing a list of entities.</returns>
    Task<TOutputList> GetAllAsync();
}

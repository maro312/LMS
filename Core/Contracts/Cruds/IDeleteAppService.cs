namespace Core.Contracts.Cruds;

/// <summary>
/// Interface for deleting an entity asynchronously.
/// </summary>
/// <typeparam name="TOutput">The output type, which must be a class.</typeparam>
/// <typeparam name="TId">The identifier type.</typeparam>
public interface IDeleteAppService<TOutput, TId>
    where TOutput : class
{
    /// <summary>
    /// Deletes an entity asynchronously by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the entity to delete.</param>
    /// <returns>A task that represents the asynchronous operation, returning the deleted entity.</returns>
    Task<TOutput> DeleteAsync(TId id);
}

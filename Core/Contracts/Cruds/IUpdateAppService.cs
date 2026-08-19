namespace Core.Contracts.Cruds;

/// <summary>
/// Interface for updating an existing entity asynchronously.
/// </summary>
/// <typeparam name="TInput">The input type, which must be a class.</typeparam>
/// <typeparam name="TOutput">The output type, which must be a class.</typeparam>
/// <typeparam name="TId">The identifier type.</typeparam>
public interface IUpdateAppService<TInput, TOutput, TId>
    where TInput : class
    where TOutput : class
{
    /// <summary>
    /// Updates an existing entity asynchronously.
    /// </summary>
    /// <param name="input">The input data for the update operation.</param>
    /// <param name="id">The identifier of the entity to update.</param>
    /// <returns>A task that represents the asynchronous update operation, returning the updated entity.</returns>
    Task<TOutput> UpdateAsync(TInput input, TId id);
}

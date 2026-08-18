namespace LMS.Domain.Repositories;

public interface IGenericRepository<TEntity, TId> where TEntity : class
{
    /// <summary>
    /// Add new entity
    /// </summary>
    Task AddAsync(TEntity entity);

    /// <summary>
    /// Edit entity
    /// </summary>
    Task UpdateAsync(TEntity entity);

    /// <summary>
    /// Get all queryable
    /// </summary>
    IQueryable<TEntity> GetAllQuerable();

    /// <summary>
    /// Get all entities asynchronously
    /// </summary>
    Task<ICollection<TEntity>> GetAllAsync();

    /// <summary>
    /// Get paginated entities asynchronously
    /// </summary>
    Task<IList<TEntity>> GetAllPagenatedAsync(int pageSize, int pageNumber);

    /// <summary>
    /// Delete entity
    /// </summary>
    Task DeleteAsync(TEntity entity);

    /// <summary>
    /// Get entity by id
    /// </summary>
    Task<TEntity?> GetByIdAsync(TId id);

    /// <summary>
    /// Get entity by id without tracking
    /// </summary>
    Task<TEntity?> GetByIdAsNoTrackingAsync(TId id);

    /// <summary>
    /// Check if entity exists
    /// </summary>
    Task<bool> ExistsAsync(TId id);

    /// <summary>
    /// Save all changes to data store
    /// </summary>
    Task<int> SaveChangesAsync();
}

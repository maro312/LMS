using LMS.Core.Contracts;
using LMS.Domain.Repositories;
using LMS.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infrastructure.Repositories;

public class Repository<TEntity, TKey> : IGenericRepository<TEntity, TKey> where TEntity : class, IEntity<TKey>
{
    private readonly LmsDbContext _context;

    public Repository(LmsDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc/>
    public async Task AddAsync(TEntity entity)
    {
        await _context.Set<TEntity>().AddAsync(entity);
    }

    /// <inheritdoc/>
    public Task UpdateAsync(TEntity entity)
    {
        _context.Set<TEntity>().Update(entity);
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public IQueryable<TEntity> GetAllQuerable()
    {
        return _context.Set<TEntity>().AsQueryable();
    }

    /// <inheritdoc/>
    public virtual async Task<ICollection<TEntity>> GetAllAsync()
    {
        return await _context.Set<TEntity>().AsNoTracking().ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<IList<TEntity>> GetAllPagenatedAsync(int pageSize, int pageNumber)
    {
        return await _context.Set<TEntity>().AsNoTracking().Skip(pageNumber * pageSize).Take(pageSize).ToListAsync();
    }

    /// <inheritdoc/>
    public Task DeleteAsync(TEntity entity)
    {
        _context.Set<TEntity>().Remove(entity);
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public virtual async Task<TEntity?> GetByIdAsync(TKey id)
    {
        return await _context.Set<TEntity>().FirstOrDefaultAsync(e => e.Id!.Equals(id));
    }

    /// <inheritdoc/>
    public virtual async Task<TEntity?> GetByIdAsNoTrackingAsync(TKey id)
    {
        return await _context.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(e => e.Id!.Equals(id));
    }

    /// <inheritdoc/>
    public Task<bool> ExistsAsync(TKey id)
    {
        return _context.Set<TEntity>().AsNoTracking().AnyAsync(e => e.Id!.Equals(id));
    }

    /// <inheritdoc/>
    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}

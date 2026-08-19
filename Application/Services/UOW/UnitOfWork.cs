using LMS.Application.Contracts.UOW;
using LMS.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore.Storage;

namespace LMS.Application.Services.UOW;

public class UnitOfWork : IUnitOfWork
{
    private readonly LmsDbContext _dbContext;
    private IDbContextTransaction? _currentTransaction;

    public UnitOfWork(LmsDbContext dbContext, IDbContextTransaction? currentTransaction = null)
    {
        _dbContext = dbContext;
        _currentTransaction = currentTransaction;
    }

    public async Task BeginTransactionAsync()
    {
        if (_currentTransaction != null)
        {
            return;
        }

        _currentTransaction = await _dbContext.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        try
        {
            await _dbContext.SaveChangesAsync();

            if (_currentTransaction != null)
            {
                await _currentTransaction.CommitAsync();
            }
        }
        catch
        {
            await RollbackTransactionAsync();
            throw;
        }
        finally
        {
            if (_currentTransaction != null)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }

    public async Task RollbackTransactionAsync()
    {
        try
        {
            if (_currentTransaction != null)
            {
                await _currentTransaction.RollbackAsync();
            }
        }
        finally
        {
            if (_currentTransaction != null)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }

    public void Dispose()
    {
        _currentTransaction?.Dispose();
        _dbContext.Dispose();
    }
}

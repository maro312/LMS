using LMS.Domain.Entities;
using LMS.Domain.Lookups;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infrastructure.DbContexts;

public class LmsDbContext : IdentityDbContext
{
    public LmsDbContext(DbContextOptions<LmsDbContext> options) : base(options)
    {
    }

    #region Lookup Entities
    public DbSet<Category> Categories { get; set; }
    #endregion

    #region Entities
    public DbSet<Book> Books { get; set; }
    public new DbSet<User> Users { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<BorrowingRequest> BorrowRequests { get; set; }
    #endregion

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(LmsDbContext).Assembly);
    }
}

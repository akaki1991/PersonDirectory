using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PersonDirectory.Domain.CityManagement;
using PersonDirectory.Domain.PersonManagement;
using PersonDirectory.Domain.PersonManagement.ReadModels;
using PersonDirectory.Shared.Infrastructure.Abstractios;
using PersonDirectory.Shared.Models;
using System.Reflection;

namespace PersonDirectory.Infrastructure.DataAccess;

public class PersonDirectoryDbContext(DbContextOptions<PersonDirectoryDbContext> options) : DbContext(options), IDbContext
{
    public DbSet<PersonRelationship> PersonRelationships { get; set; }
    public DbSet<Person> Persons { get; set; }
    public DbSet<City> Cities { get; set; }
    public DbSet<PersonReadModel> PersonReadModels { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var now = DateTimeOffset.Now;

        foreach (var item in ChangeTracker.Entries<Entity>().Where(entity => entity.State == EntityState.Added || entity.State == EntityState.Modified))
        {
            item.Entity.ChangedAt = now;
        }

        foreach (var item in ChangeTracker.Entries<Entity>().Where(entity => entity.State == EntityState.Added))
        {
            item.Entity.CreatedAt = now;
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}

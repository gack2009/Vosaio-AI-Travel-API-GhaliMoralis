using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Application.Common.Interfaces;

public interface IApplicationDbContext
{
    public DbSet<Domain.Entities.Itinerary> Itineraries { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    int SaveChanges();

    int ExecuteSql(FormattableString sql);

    EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
    void Dispose();
}
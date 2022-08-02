using HotelRentalManager.DataAccessLayer.Entities.Common;
using HotelRentalManager.DataAccessLayer.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace HotelRentalManager.DataAccessLayer;

public class DataContext : DbContext, IDataContext
{
	public DataContext(DbContextOptions<DataContext> options) : base(options)
	{
	}

	public void Delete<TEntity>(TEntity entity) where TEntity : BaseEntity
	{
		ArgumentNullException.ThrowIfNull(entity, nameof(entity));

		Set<TEntity>().Remove(entity);
	}

	public void Delete<TEntity>(IEnumerable<TEntity> entities) where TEntity : BaseEntity
	{
		ArgumentNullException.ThrowIfNull(entities, nameof(entities));

		Set<TEntity>().RemoveRange(entities);
	}

	public void Edit<TEntity>(TEntity entity) where TEntity : BaseEntity
	{
		ArgumentNullException.ThrowIfNull(entity, nameof(entity));

		Set<TEntity>().Update(entity);
	}

	public ValueTask<TEntity> GetAsync<TEntity>(params object[] keyValues) where TEntity : BaseEntity
	{
		return Set<TEntity>().FindAsync(keyValues);
	}

	public IQueryable<TEntity> GetData<TEntity>(bool ignoreQueryFilters = false, bool trackingChanges = false) where TEntity : BaseEntity
	{
		var set = Set<TEntity>().AsQueryable();

		if (ignoreQueryFilters)
		{
			set = set.IgnoreQueryFilters();
		}

		return trackingChanges ?
			set.AsTracking() :
			set.AsNoTrackingWithIdentityResolution();
	}

	public void Insert<TEntity>(TEntity entity) where TEntity : BaseEntity
	{
		ArgumentNullException.ThrowIfNull(entity, nameof(entity));

		Set<TEntity>().Add(entity);
	}

	public Task SaveAsync() => SaveChangesAsync();

	public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		var entries = ChangeTracker.Entries()
			.Where(e => e.Entity.GetType().IsSubclassOf(typeof(BaseEntity)))
			.ToList();

		foreach (var entry in entries.Where(e => e.State is EntityState.Added or EntityState.Modified or EntityState.Deleted))
		{
			BaseEntity baseEntity = (BaseEntity)entry.Entity;

			if (entry.State is EntityState.Added)
			{
				baseEntity.CreatedDate = DateTime.UtcNow;
				baseEntity.LastModifiedDate = null;

				if (baseEntity is DeletableEntity deletableEntity)
				{
					deletableEntity.IsDeleted = false;
					deletableEntity.DeletedDate = null;
				}
			}
			if (entry.State is EntityState.Modified)
			{
				baseEntity.LastModifiedDate = DateTime.UtcNow;
			}
			if (entry.State is EntityState.Deleted)
			{
				if (baseEntity is DeletableEntity deletableEntity)
				{
					entry.State = EntityState.Modified;
					deletableEntity.IsDeleted = true;
					deletableEntity.DeletedDate = DateTime.UtcNow;
				}
			}
		}

		return base.SaveChangesAsync(cancellationToken);
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
		modelBuilder.ApplyTrimStringConverter();
		modelBuilder.ApplyQueryFilter(this);
		base.OnModelCreating(modelBuilder);
	}

	private void SetQueryFilter<T>(ModelBuilder builder) where T : DeletableEntity
	{
		builder.Entity<T>().HasQueryFilter(x => !x.IsDeleted && x.DeletedDate == null);
	}
}
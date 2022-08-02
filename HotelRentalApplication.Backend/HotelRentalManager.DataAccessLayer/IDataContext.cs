using HotelRentalManager.DataAccessLayer.Entities.Common;

namespace HotelRentalManager.DataAccessLayer;

public interface IDataContext
{
    void Delete<TEntity>(TEntity entity) where TEntity : BaseEntity;

    void Delete<TEntity>(IEnumerable<TEntity> entities) where TEntity : BaseEntity;

    void Edit<TEntity>(TEntity entity) where TEntity : BaseEntity;

    ValueTask<TEntity> GetAsync<TEntity>(params object[] keyValues) where TEntity : BaseEntity;

    IQueryable<TEntity> GetData<TEntity>(bool ignoreQueryFilters = false, bool trackingChanges = false) where TEntity : BaseEntity;

    void Insert<TEntity>(TEntity entity) where TEntity : BaseEntity;

    Task SaveAsync();
}
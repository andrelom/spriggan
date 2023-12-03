namespace Spriggan.Core.Data;

public interface IRepository<T> : IDisposable where T : Entity
{
    IUnitOfWork UnitOfWork { get; }

    IQueryable<T> GetAll();

    Task<T?> Get(Guid id);

    void AddOrUpdate(T entity);

    void Delete(T entity);

    void Attach(T entity);
}

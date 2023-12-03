namespace Spriggan.Core.Data;

public abstract class Repository<TDbContext, TEntity> : IRepository<TEntity> where TDbContext : DbContext where TEntity : Entity
{
    private bool _disposed;

    protected readonly TDbContext Context;

    protected Repository(TDbContext context)
    {
        Context = context;
    }

    public IUnitOfWork UnitOfWork => Context;

    public IQueryable<TEntity> GetAll()
    {
        return Context.Set<TEntity>();
    }

    public async Task<TEntity?> Get(Guid id)
    {
        return await Context.Set<TEntity>().FindAsync(id);
    }

    public void AddOrUpdate(TEntity entity)
    {
        Context.Update(entity);
    }

    public void Delete(TEntity entity)
    {
        Context.Set<TEntity>().Remove(entity);
    }

    public void Attach(TEntity entity)
    {
        Context.Attach(entity);
    }

    #region Dispose

    public void Dispose()
    {
        Dispose(true);

        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed && disposing)
        {
            Context.Dispose();
        }

        _disposed = true;
    }

    #endregion
}

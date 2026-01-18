namespace Company.Ecommerce.Orders.Internal.Domain.Base;

internal interface IBaseRepository<T> where T : Entity
{
    Task<T?> GetByIdAsync(Guid id);
    void Add(T entity);
    void Update(T entity);
    void Delete(T entity);
    Task<bool> ExistsAsync(Guid id);
}


internal class BaseRepository<T> : IBaseRepository<T> where T : Entity
{
    protected readonly DbContext _context;
    protected readonly DbSet<T> _dbSet;

    public BaseRepository(DbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<T?> GetByIdAsync(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }

    public void Add(T entity)
    {
        var now = DateTime.UtcNow;
        entity.CreatedAt = now;
        entity.LastModifiedAt = now;
        _dbSet.Add(entity);
    }

    public void Update(T entity)
    {
        entity.LastModifiedAt = DateTime.UtcNow;
        _dbSet.Update(entity);
    }

    public void Delete(T entity)
    {
        _dbSet.Remove(entity);
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _dbSet.AnyAsync(e => e.Id == id);
    }
}

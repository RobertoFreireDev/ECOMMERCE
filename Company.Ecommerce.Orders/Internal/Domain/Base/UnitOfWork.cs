namespace Company.Ecommerce.Orders.Internal.Domain.Base;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

internal class UnitOfWork : IUnitOfWork
{
    private readonly OrdersDbContext _context;

    public UnitOfWork(OrdersDbContext context)
    {
        _context = context;
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }
}
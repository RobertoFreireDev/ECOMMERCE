namespace Company.Ecommerce.Orders.Internal.Domain.Repositories;

internal interface IOrderRepository
{
    void Add(Order order);
}

internal class OrderRepository : BaseRepository<Order>, IOrderRepository
{
    public OrderRepository(OrdersDbContext dbContext) : base(dbContext)
    {

    }
}

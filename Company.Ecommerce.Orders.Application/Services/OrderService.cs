namespace Company.Ecommerce.Orders.Application.Services;

public class OrderService : IOrderService
{
    public async Task<Guid> ProcessAsync(ProcessOrderDto request, CancellationToken cancellationToken)
    {
        return Guid.NewGuid();
    }
}
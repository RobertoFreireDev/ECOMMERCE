namespace Company.Ecommerce.Orders.Application.Services;

public class OrderService(IEventPublisher eventPublisher) : IOrderService
{
    public async Task<Guid> ProcessAsync(ProcessOrderDto request, CancellationToken cancellationToken)
    {
        var orderId = Guid.NewGuid();

        await eventPublisher.PublishAsync(
            new OrderPlacedEvent(orderId));

        return orderId;
    }
}
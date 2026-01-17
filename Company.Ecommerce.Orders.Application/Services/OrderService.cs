namespace Company.Ecommerce.Orders.Application.Services;

public class OrderService(IEventPublisher eventPublisher, IShoppingCartAccessPoint shoppingCartAccessPoint, ILogger<OrderService> logger) : IOrderService
{
    public async Task<Guid> ProcessAsync(ProcessOrderDto request, Guid customerId, CancellationToken cancellationToken)
    {
        var orderId = Guid.NewGuid();        
        var cartItems = await shoppingCartAccessPoint.GetAsync(customerId, cancellationToken);
        logger.LogInformation($"Processing order {orderId} for customer {customerId}");
        await eventPublisher.PublishAsync(new OrderPlacedEvent(orderId, customerId, cartItems));
        return orderId;
    }
}
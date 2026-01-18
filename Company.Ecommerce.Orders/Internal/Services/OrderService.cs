using Company.Ecommerce.Orders.Internal.Services.Interfaces;

namespace Company.Ecommerce.Orders.Internal.Services;

internal class OrderService(ILogger<OrderService> logger) : IOrderService
{
    public async Task<Guid> ProcessAsync(ProcessOrderRequest request, Guid customerId, CancellationToken cancellationToken)
    {
        var orderId = Guid.NewGuid();
        //var cartItems = await shoppingCartAccessPoint.GetAsync(customerId, cancellationToken);
        logger.LogInformation($"Processing order {orderId} for customer {customerId}");
        return orderId;
    }
}
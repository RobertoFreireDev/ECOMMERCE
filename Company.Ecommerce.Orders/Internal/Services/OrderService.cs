namespace Company.Ecommerce.Orders.Internal.Services;

internal class OrderService(
    IShoppingCartAccessPoint shoppingCartAccessPoint,
    ILogger<OrderService> logger) : IOrderService
{
    public async Task<Guid> ProcessAsync(ProcessOrderRequest request, Guid customerId, CancellationToken cancellationToken)
    {
        var orderId = Guid.NewGuid();
        var cartItems = await shoppingCartAccessPoint.GetShoppingCart(customerId, cancellationToken);
        logger.LogInformation($"Processing order {orderId} for customer {customerId} with {cartItems.Products.Count} items(s)");
        return orderId;
    }
}
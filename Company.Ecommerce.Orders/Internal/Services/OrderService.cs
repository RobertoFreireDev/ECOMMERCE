namespace Company.Ecommerce.Orders.Internal.Services;

internal class OrderService(
    IShoppingCartAccessPoint shoppingCartAccessPoint,
    IOrderRepository orderRepository,
    IUnitOfWork unitOfWork,
    ILogger<OrderService> logger) : IOrderService
{
    public async Task<Guid> ProcessAsync(ProcessOrderRequest request, Guid customerId, CancellationToken cancellationToken)
    {
        var orderId = Guid.NewGuid();
        var cart = await shoppingCartAccessPoint.GetShoppingCart(customerId, cancellationToken);
        logger.LogInformation($"Processing order {orderId} for customer {customerId} with {cart.Products.Count} items(s)");

        var order = new Order()
        {
            Id = orderId,
            ShippingAddressId = request.ShippingAddressId,
            BillingAddressId = request.BillingAddressId ?? request.ShippingAddressId,
            PaymentMethod = request.PaymentMethod,
            CouponCode = request.CouponCode,
            CustomerId = customerId,
            Items = cart.Products.Select(p => new OrderItem()
            {
                ProductId = p.ProductId,
                Quantity = p.Quantity,
                Name = p.Name,
                UnitPrice = p.UnitPrice
            }).ToList(),
        };

        orderRepository.Add(order);
        await unitOfWork.SaveChangesAsync();


        logger.LogInformation($"Order {orderId} saved");
        return orderId;
    }
}
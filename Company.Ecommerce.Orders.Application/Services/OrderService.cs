namespace Company.Ecommerce.Orders.Application.Services;

public class OrderService(IEventPublisher eventPublisher, ILogger<OrderService> logger) : IOrderService
{
    public async Task<Guid> ProcessAsync(ProcessOrderDto request, CancellationToken cancellationToken)
    {
        var orderId = Guid.NewGuid();

        logger.LogInformation(
            "Processing order {OrderId} with shipping address {ShippingAddressId}, billing address {BillingAddressId}, payment method {PaymentMethod}, and coupon code {CouponCode}",
            orderId,
            request.ShippingAddressId,
            request.BillingAddressId,
            request.PaymentMethod,
            request.CouponCode);

        await eventPublisher.PublishAsync(
            new OrderPlacedEvent(orderId));

        return orderId;
    }
}
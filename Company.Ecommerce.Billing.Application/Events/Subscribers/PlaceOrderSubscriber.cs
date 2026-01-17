namespace Company.Ecommerce.Billing.Application.Events.Subscribers;

public sealed class PlaceOrderSubscriber(ILogger<PlaceOrderSubscriber> logger)
    : IDomainEventHandler<OrderPlacedEvent>
{
    public async Task<bool> HandleAsync(
        OrderPlacedEvent domainEvent)
    {
        logger.LogInformation(
            "Creating invoice for order {OrderId}",
            domainEvent.OrderId);

        return false;
    }
}
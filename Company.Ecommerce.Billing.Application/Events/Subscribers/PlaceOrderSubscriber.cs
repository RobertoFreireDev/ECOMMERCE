namespace Company.Ecommerce.Billing.Application.Events.Subscribers;

public sealed class PlaceOrderSubscriber(ILogger<PlaceOrderSubscriber> logger)
    : IDomainEventHandler<OrderPlacedEvent>
{
    public async Task HandleAsync(
        OrderPlacedEvent domainEvent,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Creating invoice for order {OrderId}",
            domainEvent.OrderId);
    }
}
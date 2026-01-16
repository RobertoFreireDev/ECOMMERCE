namespace Company.Ecommerce.Billing.Application.Events.Subscribers;

public sealed class PlaceOrderSubscriber
    : IDomainEventHandler<OrderPlacedEvent>
{
    public async Task HandleAsync(
        OrderPlacedEvent domainEvent,
        CancellationToken cancellationToken)
    {
        // create invoice
    }
}
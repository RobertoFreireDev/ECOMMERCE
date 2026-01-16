namespace Company.Ecommerce.Common;

public sealed class InMemoryEventPublisher(IServiceProvider serviceProvider)
    : IEventPublisher
{
    public async Task PublishAsync<TEvent>(
        TEvent domainEvent,
        CancellationToken cancellationToken = default)
        where TEvent : IDomainEvent
    {
        var handlers = serviceProvider.GetServices<IDomainEventHandler<TEvent>>();

        foreach (var handler in handlers)
        {
            await handler.HandleAsync(domainEvent, cancellationToken);
        }
    }
}
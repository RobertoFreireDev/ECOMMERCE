namespace Company.Ecommerce.Common;

public interface IDomainEvent
{
    public EventTypes EventType { get; }
}

public abstract record DomainEvent : IDomainEvent
{
    public EventTypes EventType { get; }

    public DomainEvent(EventTypes eventType)
    {
        EventType = eventType;
    }
}

public interface IDomainEventHandler<in TEvent>
    where TEvent : IDomainEvent
{
    Task HandleAsync(TEvent domainEvent, CancellationToken cancellationToken);
}

public interface IEventPublisher
{
    Task PublishAsync<TEvent>(TEvent domainEvent, CancellationToken cancellationToken = default)
        where TEvent : IDomainEvent;
}
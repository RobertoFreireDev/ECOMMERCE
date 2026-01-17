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
    Task<bool> HandleAsync(TEvent domainEvent);
}

public interface IEventPublisher
{
    Task PublishAsync<TEvent>(TEvent domainEvent)
        where TEvent : IDomainEvent;

    Task ReplayEventAsync();
}
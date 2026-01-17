namespace Company.Ecommerce.Events;

public sealed class EventPublisher(
    IServiceProvider serviceProvider, 
    IEventRepository<EventData> _eventRepository,
    IEventRepository<FailedEvent> _failedEventRepository)
    : IEventPublisher
{
    JsonSerializerOptions Options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        WriteIndented = false,
        Converters =
        {
            new JsonStringEnumConverter()
        }
    };

    public async Task PublishAsync<TEvent>(TEvent domainEvent)
        where TEvent : IDomainEvent
    {
        var eventData = new EventData()
        {
            Id = Guid.NewGuid(),
            OccurredOnUtc = DateTime.UtcNow,
            EventType = domainEvent.EventType,
            Payload = JsonSerializer.Serialize(domainEvent, Options),
        };

        await _eventRepository.AddAsync(eventData);

        var handlers = serviceProvider.GetServices<IDomainEventHandler<TEvent>>();

        foreach (var handler in handlers)
        {
            try
            {
                var successResult = await handler.HandleAsync(domainEvent);

                if (!successResult)
                {
                    CreateFailedEvent(eventData);
                }
            }
            catch (Exception ex)
            {
                CreateFailedEvent(eventData);
            }
        }
    }

    public void CreateFailedEvent(EventData eventData)
    {
        _failedEventRepository.AddAsync(new FailedEvent()
        {
            Id = Guid.NewGuid(),
            EventId = eventData.Id,
            Event = eventData,
        });
    }
}
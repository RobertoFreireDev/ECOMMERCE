namespace Company.Ecommerce.Events;

public sealed class EventPublisher(
    IServiceProvider serviceProvider, 
    IEventRepository<EventData> _eventRepository,
    IEventRepository<FailedEvent> _failedEventRepository,
    IEventRepository<DeadLetterEvent> _deadLetterEventRepository)
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
        await HandleEvent(domainEvent, async e =>
        {
            await CreateFailedEventAsync(eventData);
        });
    }

    public async Task ReplayEventAsync()
    {
        var failedEvent = await _failedEventRepository.GetTopAsync(
                condition: null,
                orderBy: e => e.OrderBy(e => e.LastAttemptUtc),
                includes: e => e.Event);

        if (failedEvent?.Event is null)
        {
            return;
        }

        if (failedEvent.Event.EventType == EventTypes.OrderPlaced)
        {
            var domainEvent = JsonSerializer.Deserialize<OrderPlacedEvent>(failedEvent.Event.Payload, Options);

            if (domainEvent is null)
            {
                await CreateDeadLetterEventAsync(failedEvent.Event);
                return;
            }

            await HandleEvent(domainEvent, async e =>
            {
                await CreateDeadLetterEventAsync(failedEvent.Event);
            });

            await _failedEventRepository.DeleteAsync(failedEvent);
        }
        else
        {
            throw new NotSupportedException($"Event type {failedEvent.Event.EventType} is not supported for replay.");
        }
    }

    private async Task HandleEvent<TEvent>(TEvent domainEvent, Func<TEvent, Task> onFailed)
        where TEvent : IDomainEvent
    {
        var handlers = serviceProvider.GetServices<IDomainEventHandler<TEvent>>();

        foreach (var handler in handlers)
        {
            try
            {
                var successResult = await handler.HandleAsync(domainEvent);

                if (!successResult)
                {
                    await onFailed(domainEvent);
                }
            }
            catch (Exception ex)
            {
                await onFailed(domainEvent);
            }
        }
    }

    private async Task CreateFailedEventAsync(EventData eventData)
    {
        await _failedEventRepository.AddAsync(new FailedEvent()
        {
            Id = Guid.NewGuid(),
            EventId = eventData.Id,
            Event = eventData,
            LastAttemptUtc = DateTime.UtcNow,
        });
    }

    private async Task CreateDeadLetterEventAsync(EventData eventData)
    {
        await _deadLetterEventRepository.AddAsync(new DeadLetterEvent()
        {
            Id = Guid.NewGuid(),
            EventId = eventData.Id,
            Event = eventData,
            FailedOnUtc = DateTime.UtcNow,
        });
    }
}
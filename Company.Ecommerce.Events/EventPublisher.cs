namespace Company.Ecommerce.Events;

public sealed class EventPublisher(IServiceProvider serviceProvider, IEventDataRepository _repository, ILogger<EventPublisher> logger)
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

    public async Task PublishAsync<TEvent>(
        TEvent domainEvent,
        CancellationToken cancellationToken = default)
        where TEvent : IDomainEvent
    {
        string payload = string.Empty;

        try
        {
            payload = JsonSerializer.Serialize(domainEvent, Options);
            await _repository.AddAsync(new EventData()
            {
                Id = Guid.NewGuid(),
                OccurredOnUtc = DateTime.UtcNow,
                EventType = domainEvent.EventType,
                Payload = payload,
            }, cancellationToken);

            var handlers = serviceProvider.GetServices<IDomainEventHandler<TEvent>>();

            foreach (var handler in handlers)
            {
                await handler.HandleAsync(domainEvent, cancellationToken);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Error handling event: {payload}");
            throw;
        }
    }
}
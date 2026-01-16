namespace Company.Ecommerce.Common.Events;

public sealed record OrderPlacedEvent(Guid OrderId)
    : DomainEvent;

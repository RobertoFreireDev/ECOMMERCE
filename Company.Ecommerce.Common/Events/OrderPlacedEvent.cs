namespace Company.Ecommerce.Common.Events;

public sealed record OrderPlacedEvent(Guid OrderId, Guid customerId, List<CartItemDto> cartItems) : DomainEvent(EventTypes.OrderPlaced);

namespace Company.Ecommerce.Common;

public abstract record DomainEvent : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
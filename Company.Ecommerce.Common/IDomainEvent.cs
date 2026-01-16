namespace Company.Ecommerce.Common;

public interface IDomainEvent
{
    DateTime OccurredOn { get; }
}

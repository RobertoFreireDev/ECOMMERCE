namespace Company.Ecommerce.Orders.Internal.Domain.Base;

internal class Entity
{
    public Guid Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime LastModifiedAt { get; set; }
}
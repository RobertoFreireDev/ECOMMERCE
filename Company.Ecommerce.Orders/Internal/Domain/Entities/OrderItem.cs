namespace Company.Ecommerce.Orders.Internal.Domain.Entities;

internal class OrderItem
{
    public required Guid ProductId { get; set; }
    public required string Name { get; set; }
    public required int UnitPrice { get; set; }
    public required int Quantity { get; set; }
}

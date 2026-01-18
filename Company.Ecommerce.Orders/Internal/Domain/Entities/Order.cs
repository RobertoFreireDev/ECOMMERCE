namespace Company.Ecommerce.Orders.Internal.Domain.Entities;

internal class Order : Entity
{
    public required Guid ShippingAddressId { get; init; }

    public required Guid BillingAddressId { get; init; }

    public required string PaymentMethod { get; init; }

    public string? CouponCode { get; init; }

    public required Guid CustomerId { get; set; }

    public required List<OrderItem> Items { get; set; }
}

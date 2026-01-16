namespace Company.Ecommerce.Orders.Application.Dtos;

public class ProcessOrderDto
{    
    public Guid ShippingAddressId { get; init; }

    public Guid? BillingAddressId { get; init; }

    public string PaymentMethod { get; init; }

    public string? CouponCode { get; init; }
}

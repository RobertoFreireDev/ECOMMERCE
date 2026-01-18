namespace Company.Ecommerce.Orders.Internal.Controllers.DataContracts.Requests;

internal class ProcessOrderRequest
{
    /// <summary>
    /// Shipping address selected by the customer
    /// </summary>
    [Required]
    public Guid ShippingAddressId { get; init; }

    /// <summary>
    /// Billing address (optional, defaults to shipping address)
    /// </summary>
    public Guid? BillingAddressId { get; init; }

    /// <summary>
    /// Selected payment method
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string PaymentMethod { get; init; }

    /// <summary>
    /// Applied promotion or coupon code
    /// </summary>
    [MaxLength(20)]
    public string? CouponCode { get; init; }
}
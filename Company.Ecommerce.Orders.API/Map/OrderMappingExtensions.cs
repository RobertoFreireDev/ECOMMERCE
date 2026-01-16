namespace Company.Ecommerce.Orders.API.Map;

public static class OrderMappingExtensions
{
    public static ProcessOrderDto ToDto(this ProcessOrderRequest model)
    {
        return new ProcessOrderDto
        {
            ShippingAddressId = model.ShippingAddressId,
            BillingAddressId = model.BillingAddressId,
            PaymentMethod = model.PaymentMethod,
            CouponCode = model.CouponCode
        };
    }
}

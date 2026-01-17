namespace Company.Ecommerce.Common.AccessPoints;

public interface IShoppingCartAccessPoint
{
    Task<List<CartItemDto>> GetAsync(Guid customerId, CancellationToken ct);
}

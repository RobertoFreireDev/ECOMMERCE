namespace Company.Ecommerce.ShoppingCart.API.AccessPoints;

public class ShoppingCartAccessPoint : IShoppingCartAccessPoint
{
    public async Task<List<CartItemDto>> GetAsync(Guid customerId, CancellationToken ct)
    {
        var items = new List<CartItemDto>
        {
            new CartItemDto() { ProductId = Guid.NewGuid(), Quantity = 2, UnitPrice = 1000 },
            new CartItemDto() { ProductId = Guid.NewGuid(), Quantity = 1, UnitPrice = 1299 },
        };

        return items;
    }
}

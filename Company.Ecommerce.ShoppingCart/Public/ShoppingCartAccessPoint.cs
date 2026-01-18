namespace Company.Ecommerce.ShoppingCart.Public;

public interface IShoppingCartAccessPoint
{
    Task<ShoppingCartResponse> GetShoppingCart(Guid CustomerId, CancellationToken cancellationToken);
}

public class ShoppingCartAccessPoint : IShoppingCartAccessPoint
{
    public async Task<ShoppingCartResponse> GetShoppingCart(Guid customerId, CancellationToken cancellationToken)
    {
        return new ShoppingCartResponse()
        {
            CartId = Guid.NewGuid(),
            CustomerId = customerId,
            Products = new List<ShoppingCartProduct>
            {
                new ShoppingCartProduct()
                {
                    ProductId = Guid.NewGuid(),
                    Name = "Keyboard",
                    UnitPrice = 250.00m,
                    Quantity = 1
                },
                new ShoppingCartProduct()
                {
                    ProductId = Guid.NewGuid(),
                    Name = "Mouse",
                    UnitPrice = 120.00m,
                    Quantity = 2
                }
            }
        };
    }
}

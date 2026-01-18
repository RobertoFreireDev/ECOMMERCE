namespace Company.Ecommerce.ShoppingCart.Public.DataContracts.Responses;

public class ShoppingCartProduct
{
    public Guid ProductId { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
}

public class ShoppingCartResponse
{
    public Guid CartId { get; set; }

    public Guid CustomerId { get; set; }

    public List<ShoppingCartProduct> Products { get; set; } = [];
}

namespace Company.Ecommerce.Common.Dto;

public class CartItemDto
{
    public Guid ProductId { get; init; }

    public int Quantity { get; init; }

    public int UnitPrice { get; init; }
}
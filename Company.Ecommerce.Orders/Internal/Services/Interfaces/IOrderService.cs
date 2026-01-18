namespace Company.Ecommerce.Orders.Internal.Services.Interfaces;

internal interface IOrderService
{
    Task<Guid> ProcessAsync(ProcessOrderRequest request, Guid customerId, CancellationToken cancellationToken);
}
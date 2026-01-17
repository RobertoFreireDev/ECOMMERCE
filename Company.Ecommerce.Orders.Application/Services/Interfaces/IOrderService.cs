namespace Company.Ecommerce.Orders.Application.Services.Interfaces;

public interface IOrderService
{
    Task<Guid> ProcessAsync(ProcessOrderDto request, Guid customerId, CancellationToken cancellationToken);
}
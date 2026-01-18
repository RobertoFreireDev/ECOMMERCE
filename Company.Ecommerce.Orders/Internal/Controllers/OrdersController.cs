namespace Company.Ecommerce.Orders.Internal.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
internal class OrdersController(IOrderService orderService) : ControllerBase
{
    /// <summary>
    /// Processes a new order from the customer's cart
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateOrder([FromBody] ProcessOrderRequest request, CancellationToken cancellationToken)
    {
        // TO DO: Get customerid from cookies
        var customerId = Guid.NewGuid();
        return Ok(await orderService.ProcessAsync(request, customerId, cancellationToken));
    }
}
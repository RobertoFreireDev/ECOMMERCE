namespace Company.Ecommerce.Orders.API.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public sealed class OrdersController(IOrderService orderService) : ControllerBase
{
    /// <summary>
    /// Processes a new order from the customer's cart
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateOrder([FromBody] ProcessOrderRequest request, CancellationToken cancellationToken)
    {
        return Ok(await orderService.ProcessAsync(request.ToDto(), cancellationToken));
    }
}
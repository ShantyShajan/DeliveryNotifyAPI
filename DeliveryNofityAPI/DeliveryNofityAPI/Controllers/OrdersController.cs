using DeliveryNofityAPI.Models;
using DeliveryNofityAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryNofityAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost("GetRecentOrder")]
        public async Task<IActionResult> GetRecentOrder([FromBody] CustomerRequest request)
        {
            try
            {
                var (customer, order) = await _orderService.GetCustomerOrderAsync(request.User, request.CustomerId);

                if (order == null)
                {
                    return Ok(new { Customer = customer, Order = (Order)null });
                }

                return Ok(new { Customer = customer, Order = order });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }

    public class CustomerRequest
    {
        public string User { get; set; }
        public int CustomerId { get; set; }
    }
}

using DependencyInjectionExercise.Data;
using DependencyInjectionExercise.Models;
using DependencyInjectionExercise.Services;
using DependencyInjectionExercise.Services.BookServices;
using DependencyInjectionExercise.Services.OrderServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DependencyInjectionExercise.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;
    private readonly DiscountService _discountService;
    private readonly OrderTrackingService _orderTracking;

    public OrdersController(
        IOrderService orderService,
        DiscountService discountService,
        OrderTrackingService orderTracking)
    {
        _orderService = orderService;
        _discountService = discountService;
        _orderTracking = orderTracking;
    }

    [HttpPost]
    public async Task<ActionResult<Order>> PlaceOrder(Order order)
    {
        var result = await _orderService.PlaceOrderAsync(order);

        return CreatedAtAction(nameof(GetOrder), new { id = result.Id }, result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Order>> GetOrder(int id)
    {
        return await _orderService.GetOrderAsync(id);
    }

    [HttpGet]
    public async Task<ActionResult<List<Order>>> GetOrders()
    {
        return await _orderService.GetOrdersAsync();
    }

    [HttpPatch("{id}/cancel")]
    public async Task<IActionResult> CancelOrder(int id)
    {
        await _orderService.CancelOrderAsync(id);

        return NoContent();
    }

    [HttpGet("debug/discount-state")]
    public ActionResult GetDiscountDebugState()
    {
        return Ok(new
        {
            _discountService.LastCustomerName,
            _discountService.LastAppliedDiscount,
            _discountService.OrderCountInSession
        });
    }

    [HttpGet("debug/tracking-instance")]
    public ActionResult GetTrackingDebugState()
    {
        return Ok(new
        {
            _orderTracking.InstanceId,
            TrackingNote = _orderTracking.GetTrackingNote(),
            Warning = "If TrackingNote is null, that's a bug!"
        });
    }    
}

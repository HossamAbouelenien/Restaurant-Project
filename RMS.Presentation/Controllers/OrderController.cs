using Microsoft.AspNetCore.Mvc;
using RMS.ServicesAbstraction;
using RMS.Shared;
using RMS.Shared.DTOs.MenuItemsDTOs;
using RMS.Shared.DTOs.OrderDTOs;
using RMS.Shared.QueryParams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreateOrderDTO orderDto)
        {
            try
            {
                var createdOrder = await _orderService.CreateOrderAsync(orderDto);
                //return CreatedAtAction(nameof(GetOrderById), new { id = createdOrder.Id }, createdOrder);
                return Ok(createdOrder);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult<PaginatedResult<OrderDTO>>> GetAllOrders([FromQuery] OrderQueryParams queryParams)
        {
            try
            {
                var result = await _orderService.GetAllOrdersAsync(queryParams);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDetailsDTO>> GetOrderById(int id)
        {
            try
            {
                var orderDetails = await _orderService.GetOrderByIdAsync(id);
                if (orderDetails == null)
                {
                    return NotFound();
                }
                return Ok(orderDetails);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        }
}

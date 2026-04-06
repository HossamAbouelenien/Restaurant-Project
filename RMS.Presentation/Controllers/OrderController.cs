using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMS.ServicesAbstraction;
using RMS.Shared;
using RMS.Shared.DTOs.MenuItemsDTOs;
using RMS.Shared.DTOs.OrderDTOs;
using RMS.Shared.DTOs.Utility;
using RMS.Shared.QueryParams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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


        [HttpGet("my")]
        [Authorize(Roles = SD.Role_Customer)]
        public async Task<ActionResult<PaginatedResult<OrderDTO>>> GetCustomerOrdersHistory([FromQuery] OrderQueryParams queryParams)
        {
            try
            {
                var customerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(customerId))
                {
                    return Unauthorized();
                }
                var result = await _orderService.GetCustomerOrdersHistoryAsync(queryParams, customerId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPut("{orderId}/status")]
        public async Task<ActionResult<OrderDTO>> UpdateOrderStatus(int orderId, [FromBody] string newStatus)
        {
            try
            {
                var updatedOrder = await _orderService.UpdateOrderStatusAsync(orderId, newStatus);
                return Ok(updatedOrder);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpPost("{orderId}/items")]
        public async Task<ActionResult<AddedItemsDTO>> AddItemsToOrder(int orderId, [FromBody] List<OrderItemDTO> items)
        {
            try
            {
                var addedItems = await _orderService.AddItemsToOrderAsync(orderId, items);
                return Ok(addedItems);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

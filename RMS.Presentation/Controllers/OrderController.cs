using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RMS.ServicesAbstraction.IServices.IOrderServices;
using RMS.Shared;
using RMS.Shared.DTOs.OrderDTOs;
using RMS.Shared.DTOs.Utility;
using RMS.Shared.QueryParams;
using System.Security.Claims;

namespace RMS.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ILogger<OrderController> _logger;

        public OrderController(IOrderService orderService, ILogger<OrderController> logger)
        {
            _orderService = orderService;
            _logger = logger;
        }
        
        //[Authorize(Roles = SD.Role_Admin + "" + SD.Role_Customer + "" + SD.Role_Waiter + "" + SD.Role_Cashier)]
        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreateOrderDTO orderDto)
        {
                  _logger.LogInformation("CreateOrder request started");
        
                  var createdOrder = await _orderService.CreateOrderAsync(orderDto);
                //return CreatedAtAction(nameof(GetOrderById), new { id = createdOrder.Id }, createdOrder);
                return Ok(createdOrder);
        
        }

        //[Authorize(Roles = SD.Role_Admin + "" + SD.Role_Waiter + "" + SD.Role_Cashier)]
        [HttpGet]
        public async Task<ActionResult<PaginatedResult<OrderDTO>>> GetAllOrders([FromQuery] OrderQueryParams queryParams)
        {
            _logger.LogInformation("GetAllOrders request started");
            var result = await _orderService.GetAllOrdersAsync(queryParams);
                return Ok(result);
            
         
        }

        //[Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDetailsDTO>> GetOrderById(int id)
        {
            _logger.LogInformation("GetOrderById request started");
            var orderDetails = await _orderService.GetOrderByIdAsync(id);
               
                return Ok(orderDetails);
          
        }


        [HttpGet("my")]
        //[Authorize(Roles = SD.Role_Customer)]
        public async Task<ActionResult<PaginatedResult<OrderDTO>>> GetCustomerOrdersHistory([FromQuery] OrderQueryParams queryParams)
        {
            _logger.LogInformation("GetCustomerOrdersHistory request started");
            var customerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(customerId))
                {
                    _logger.LogWarning("Customer ID not found in claims");
                return Unauthorized();
                }

                var result = await _orderService.GetCustomerOrdersHistoryAsync(queryParams, customerId);
                return Ok(result);
            
           

        }

        [HttpGet("myactive")]
        //[Authorize(Roles = SD.Role_Customer)]
        public async Task<ActionResult<PaginatedResult<MyDeliveryActiveCustomersDTO>>> GetCustomerActiveOrdersHistory([FromQuery] OrderQueryParams queryParams)
        {
            _logger.LogInformation("GetCustomerActiveOrdersHistory request started");

            var customerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(customerId))
                {
                _logger.LogWarning("CustomerId is null");
                return Unauthorized();
                }
                var result = await _orderService.GetCustomerOrdersActiveAsync(queryParams, customerId);
                return Ok(result);
            
          

        }

        //[Authorize(Roles = SD.Role_Admin + "" + SD.Role_Waiter + "" + SD.Role_Cashier)]
        [HttpPut("{orderId}/status")]
        public async Task<ActionResult<OrderDTO>> UpdateOrderStatus(int orderId, [FromBody] string newStatus)
        {
            _logger.LogInformation("UpdateOrderStatus request started");
            var updatedOrder = await _orderService.UpdateOrderStatusAsync(orderId, newStatus);
                return Ok(updatedOrder);
          
        }

        //[Authorize(Roles = SD.Role_Admin + "" + SD.Role_Waiter )]
        [HttpPost("{orderId}/items")]
        public async Task<ActionResult<AddedItemsDTO>> AddItemsToOrder(int orderId, [FromBody] List<CreateOrderItemDTO> items)
        {
            _logger.LogInformation("AddItemsToOrder request started");
            var addedItems = await _orderService.AddItemsToOrderAsync(orderId, items);
                return Ok(addedItems);
            
        }


        [Authorize(Roles = SD.Role_Admin + "" + SD.Role_Waiter )]
        [HttpDelete("{orderId}/items/{itemId}")]

        public async Task<ActionResult<OrderDTO>> RemoveItemsFromOrder(int orderId, int itemId)
        {
            _logger.LogInformation("RemoveItemsFromOrder request started");

            var updatedOrder = await _orderService.RemoveItemsFromOrderAsync(orderId, itemId);
                return Ok(updatedOrder);
          
        }



        [Authorize(Roles = SD.Role_Admin + "" + SD.Role_Customer )]
        [HttpPatch("{Id}/cancel")]
        public async Task<IActionResult> CancelOrder(int Id)
        {

            _logger.LogInformation("CancelOrder request started");
            await _orderService.CancelOrderAsync(Id);
                return NoContent();
          
        }


        [Authorize(Roles = SD.Role_Admin + "" + SD.Role_Waiter + "" + SD.Role_Cashier)]
        [HttpPatch("{orderId}/mark-paid")]
        public async Task<IActionResult> MarkAsPaid(int orderId)
        {
            _logger.LogInformation("MarkOrderAsPaid request started");
            var result = await _orderService.MarkOrderAsPaidAsync(orderId);
            return Ok(result);
        }


    }













}






























using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMS.ServicesAbstraction;
using RMS.ServicesAbstraction.IDeliveryServices;
using RMS.Shared;
using RMS.Shared.DTOs.BranchStockDTOs;
using RMS.Shared.DTOs.DeliveryDTOs;
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
    [Route("api/[Controller]")]
    public class DeliveryController : ControllerBase
    {
        private readonly IDeliveryService _deliveryService;

        public DeliveryController(IDeliveryService deliveryService)
        {
            _deliveryService = deliveryService;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<PaginatedResult<DeliveryDetailsDto>>> GetAllDeliveries([FromQuery] DeliveryQueryParams queryParams)
        {
            var deliveries = await _deliveryService.GetAllDeliveriesAsync(queryParams);
            return Ok(deliveries);
        }

        [HttpGet("OwnAssignedDeliveries")]
        public async Task<ActionResult<IEnumerable<DeliveryDetailsDto>>> GetOwnAssignedDeliveriesAsync()
        {
            var deliveries = await _deliveryService.GetOwnAssignedDeliveriesAsync();
            return Ok(deliveries);

        }

        [HttpGet("DeliveryById")]
        public async Task<ActionResult<DeliveryDetailsDto>> GetDeliveryByIdAsync([FromQuery] int id)
        {
            var delivery = await _deliveryService.GetDeliveryByIdAsync(id);
            if (delivery == null)
            {
                return NotFound($"Delivery with ID {id} not found.");
            }
            return Ok(delivery);
        }

        [HttpPost("assign")]
        public async Task<ActionResult<DeliveryDetailsDto>> AssignDelivery([FromBody] AssignDeliveryDto dto)
        {
            var result = await _deliveryService.AssignDriverAsync(dto);

            return Ok(result);
        }

       
           

         [HttpPatch("{id}/status")]
         //[Authorize(Roles = "Driver,Admin")]
         public async Task<ActionResult<DeliveryDetailsDto>> UpdateStatus(int id,[FromBody] UpdateDeliveryStatusDto dto)    
         {
               
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
             if (userId == null)
                  return Unauthorized();
                
             var isAdmin = User.IsInRole(SD.Role_Admin);
             var result = await _deliveryService.UpdateDeliveryStatusAsync(id, dto,userId,isAdmin );    
             return Ok(result);
         }
        
    }

}

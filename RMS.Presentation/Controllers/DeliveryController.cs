using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMS.Services.Exceptions;
using RMS.ServicesAbstraction.IServices.IDeliveryServices;
using RMS.Shared;
using RMS.Shared.DTOs.DeliveryDTOs;
using RMS.Shared.DTOs.Utility;
using RMS.Shared.QueryParams;
using System.Security.Claims;

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

        [Authorize(Roles = SD.Role_Admin)]
        [HttpGet("GetAll")]
        public async Task<ActionResult<PaginatedResult<DeliveryDetailsDto>>> GetAllDeliveries([FromQuery] DeliveryQueryParams queryParams)
        {
            var deliveries = await _deliveryService.GetAllDeliveriesAsync(queryParams);
            return Ok(deliveries);
        }

        [Authorize(Roles = SD.Role_Driver   +"" + SD.Role_Admin)]
        [HttpGet("OwnAssignedDeliveries")]
        public async Task<ActionResult<IEnumerable<DeliveryDetailsDto>>> GetOwnAssignedDeliveriesAsync()
        {
            var deliveries = await _deliveryService.GetOwnAssignedDeliveriesAsync();
            return Ok(deliveries);

        }

        [Authorize(Roles = SD.Role_Driver + "" + SD.Role_Admin + "" + SD.Role_Customer)]
        [HttpGet("DeliveryById")]
        public async Task<ActionResult<DeliveryDetailsDto>> GetDeliveryByIdAsync([FromQuery] int id)
        {
            var delivery = await _deliveryService.GetDeliveryByIdAsync(id);
            return Ok(delivery);
        }


        [Authorize(Roles = SD.Role_Driver + "" + SD.Role_Admin)]
        [HttpPost("assign")]
        public async Task<ActionResult<DeliveryDetailsDto>> AssignDelivery([FromBody] AssignDeliveryDto dto)
        {
            var result = await _deliveryService.AssignDriverAsync(dto);

            return Ok(result);
        }



        [Authorize(Roles = SD.Role_Driver + "" + SD.Role_Admin)]
        [HttpPatch("{id}/status")]
        
         public async Task<ActionResult<DeliveryDetailsDto>> UpdateStatus(int id,[FromBody] UpdateDeliveryStatusDto dto)    
         {
               
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

             if (userId == null)
                throw new UnauthorizedDriverException();

            var isAdmin = User.IsInRole(SD.Role_Admin);
             var result = await _deliveryService.UpdateDeliveryStatusAsync(id, dto,userId,isAdmin );    
             return Ok(result);
         }


        [Authorize(Roles = SD.Role_Driver + "" + SD.Role_Admin)]
        [HttpGet("unassigned")]
        public async Task<IActionResult> GetUnAssignedDeliveries()
        {
            var result = await _deliveryService.GetUnAssignedDeliveriesAsync();

            return Ok(result);
        }


        [Authorize(Roles = SD.Role_Driver + "" + SD.Role_Admin)]
        [HttpGet("available-drivers")]
        public async Task<ActionResult<PaginatedResult<AvailableDriverDto>>> GetAvailableDrivers([FromQuery] AvailableDriversQueryParams query)
        {
            var result = await _deliveryService.GetAvailableDriversAsync(query);
            return Ok(result);
        }


    }

}

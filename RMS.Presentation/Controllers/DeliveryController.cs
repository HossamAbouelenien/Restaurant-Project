using Microsoft.AspNetCore.Mvc;
using RMS.ServicesAbstraction;
using RMS.ServicesAbstraction.IDeliveryServices;
using RMS.Shared;
using RMS.Shared.DTOs.BranchStockDTOs;
using RMS.Shared.DTOs.DeliveryDTOs;
using RMS.Shared.QueryParams;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public async Task<ActionResult<PaginatedResult<DeliveryDto>>> GetAllDeliveries([FromQuery] DeliveryQueryParams queryParams)
        {
            var deliveries = await _deliveryService.GetAllDeliveriesAsync(queryParams);
            return Ok(deliveries);
        }

        [HttpGet("OwnAssignedDeliveries")]
        public async Task<ActionResult<IEnumerable<DeliveryDto>>> GetOwnAssignedDeliveriesAsync()
        {
            var deliveries = await _deliveryService.GetOwnAssignedDeliveriesAsync();
            return Ok(deliveries);

        }
    }

}

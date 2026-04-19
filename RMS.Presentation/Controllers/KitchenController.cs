using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMS.ServicesAbstraction;
using RMS.ServicesAbstraction.IKitchenServices;
using RMS.Shared.DTOs.BranchStockDTOs;
using RMS.Shared.DTOs.KitchenDTOs;
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
    public class KitchenController : ControllerBase
    {
        private readonly IKitchenService _kitchenService;

        public KitchenController(IKitchenService kitchenService)
        {
            _kitchenService = kitchenService;
        }
        [HttpGet("KitchenTickets")]
        public async Task<ActionResult<KitchenBoardDto>> GetAllKitchenTicketsGroupedByStatusForCurrentBranchAsync([FromQuery] KitchenTicketQueryParams queryParams)
        {
            var KitchenTickets = await _kitchenService.GetAllKitchenTicketsGroupedByStatusForCurrentBranchAsync(queryParams);
            return Ok(KitchenTickets);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<KitchenTicketDetailsDto>> GetSingleKitchenTicketWithsOrderItemsAsync(int id)
        {
            var KitchenTicket = await _kitchenService.GetSingleKitchenTicketWithsOrderItemsAsync(id);
            return Ok(KitchenTicket);
        }

        [HttpGet("ActiveStations")]
        public async Task<ActionResult<List<ActivePendingStationsDTOs>>> GetListOfActiveStationsWithPendingCountAsync([FromQuery] int branchId)
        {
            var ActiveStationsWithPendingCount = await _kitchenService.GetListOfActiveStationsWithPendingCountAsync(branchId);
            return Ok(ActiveStationsWithPendingCount);
        }

        [HttpPut("{ticketId}")]
        public async Task<IActionResult> UpdateTicketStatus(int ticketId,[FromBody] UpdateTicketStatusRequestDto dto)             
        {
            var result = await _kitchenService.UpdateTicketStatusAsync(ticketId, dto);
            return Ok(result);
        }



        [HttpPatch("ConfirmServed/{id}")]
        public async Task<IActionResult> UpdateCofirmServeredColumn(int id)
        {
            var result = await _kitchenService.UpdateCofirmServeredColumn(id);
            return Ok(result);



        }
    }
}

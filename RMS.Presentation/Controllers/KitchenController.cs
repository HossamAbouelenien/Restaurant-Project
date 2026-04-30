using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMS.ServicesAbstraction.IServices.IKitchenServices;
using RMS.Shared.DTOs.KitchenDTOs;
using RMS.Shared.DTOs.Utility;
using RMS.Shared.QueryParams;

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

        [Authorize(Roles = SD.Role_Admin + "" + SD.Role_Waiter + "" + SD.Role_Chef)]
        [HttpGet("KitchenTickets")]
        public async Task<ActionResult<KitchenBoardDto>> GetAllKitchenTicketsGroupedByStatusForCurrentBranchAsync([FromQuery] KitchenTicketQueryParams queryParams)
        {
            var KitchenTickets = await _kitchenService.GetAllKitchenTicketsGroupedByStatusForCurrentBranchAsync(queryParams);
            return Ok(KitchenTickets);
        }


        [Authorize(Roles = SD.Role_Admin + "" + SD.Role_Chef)]
        [HttpGet("{id}")]
        public async Task<ActionResult<KitchenTicketDetailsDto>> GetSingleKitchenTicketWithsOrderItemsAsync(int id)
        {
            var KitchenTicket = await _kitchenService.GetSingleKitchenTicketWithsOrderItemsAsync(id);
            return Ok(KitchenTicket);
        }


        [Authorize(Roles = SD.Role_Admin + "" + SD.Role_Chef)]

        [HttpGet("ActiveStations")]
        public async Task<ActionResult<List<ActivePendingStationsDTOs>>> GetListOfActiveStationsWithPendingCountAsync([FromQuery] int branchId)
        {
            var ActiveStationsWithPendingCount = await _kitchenService.GetListOfActiveStationsWithPendingCountAsync(branchId);
            return Ok(ActiveStationsWithPendingCount);
        }

        [Authorize(Roles = SD.Role_Chef)]
        [HttpPut("{ticketId}")]
        public async Task<IActionResult> UpdateTicketStatus(int ticketId,[FromBody] UpdateTicketStatusRequestDto dto)             
        {
            var result = await _kitchenService.UpdateTicketStatusAsync(ticketId, dto);
            return Ok(result);
        }


        [Authorize(Roles = SD.Role_Waiter)]
        [HttpPatch("ConfirmServed/{id}")]
        public async Task<IActionResult> UpdateCofirmServeredColumn(int id)
        {
            var result = await _kitchenService.UpdateCofirmServeredColumn(id);
            return Ok(result);

        }


    }




}




using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<KitchenController> _logger;

        public KitchenController(IKitchenService kitchenService, ILogger<KitchenController> logger)
        {
            _kitchenService = kitchenService;
            _logger = logger;
        }

        [Authorize(Roles = SD.Role_Admin + "" + SD.Role_Waiter + "" + SD.Role_Chef)]
        [HttpGet("KitchenTickets")]
        public async Task<ActionResult<KitchenBoardDto>> GetAllKitchenTicketsGroupedByStatusForCurrentBranchAsync([FromQuery] KitchenTicketQueryParams queryParams)
        {
            _logger.LogInformation("GetKitchenTickets request started");
            var KitchenTickets = await _kitchenService.GetAllKitchenTicketsGroupedByStatusForCurrentBranchAsync(queryParams);
            return Ok(KitchenTickets);
        }


        [Authorize(Roles = SD.Role_Admin + "" + SD.Role_Chef)]
        [HttpGet("{id}")]
        public async Task<ActionResult<KitchenTicketDetailsDto>> GetSingleKitchenTicketWithsOrderItemsAsync(int id)
        {
            _logger.LogInformation("GetKitchenTicketById request started");
            var KitchenTicket = await _kitchenService.GetSingleKitchenTicketWithsOrderItemsAsync(id);
            return Ok(KitchenTicket);
        }


        [Authorize(Roles = SD.Role_Admin + "" + SD.Role_Chef)]

        [HttpGet("ActiveStations")]
        public async Task<ActionResult<List<ActivePendingStationsDTOs>>> GetListOfActiveStationsWithPendingCountAsync([FromQuery] int branchId)
        {
            _logger.LogInformation("GetActiveStations request started");
            var ActiveStationsWithPendingCount = await _kitchenService.GetListOfActiveStationsWithPendingCountAsync(branchId);
            return Ok(ActiveStationsWithPendingCount);
        }

        [Authorize(Roles = SD.Role_Chef)]
        [HttpPut("{ticketId}")]
        public async Task<IActionResult> UpdateTicketStatus(int ticketId,[FromBody] UpdateTicketStatusRequestDto dto)             
        {
            _logger.LogInformation("UpdateTicketStatus request started");
            var result = await _kitchenService.UpdateTicketStatusAsync(ticketId, dto);
            return Ok(result);
        }


        [Authorize(Roles = SD.Role_Waiter)]
        [HttpPatch("ConfirmServed/{id}")]
        public async Task<IActionResult> UpdateCofirmServeredColumn(int id)
        {
            _logger.LogInformation("ConfirmServed request started");
            var result = await _kitchenService.UpdateCofirmServeredColumn(id);
            return Ok(result);

        }


    }




}




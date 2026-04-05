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
        [HttpGet("KitchenTicketsGroupedByStatusForCurrentBranch")]
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

        [HttpGet("ActiveStationsWithPendingCount")]
        public async Task<ActionResult<List<ActivePendingStationsDTOs>>> GetListOfActiveStationsWithPendingCountAsync([FromQuery] int branchId)
        {
            var ActiveStationsWithPendingCount = await _kitchenService.GetListOfActiveStationsWithPendingCountAsync(branchId);
            return Ok(ActiveStationsWithPendingCount);
        }

    }
}

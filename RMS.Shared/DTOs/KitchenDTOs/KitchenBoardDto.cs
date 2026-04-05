using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Shared.DTOs.KitchenDTOs
{
    public class KitchenBoardDto
    {
        public List<OrderKitchenTicketDTO> Pending { get; set; } = new();
        public List<OrderKitchenTicketDTO> Preparing { get; set; } = new();
        public List<OrderKitchenTicketDTO> Done { get; set; } = new();
    }
}

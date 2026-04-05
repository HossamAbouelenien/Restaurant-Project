using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Shared.DTOs.KitchenDTOs
{
    public class KitchenBoardDto
    {
        public List<KitchenTicketDTO> Pending { get; set; } = new();
        public List<KitchenTicketDTO> Preparing { get; set; } = new();
        public List<KitchenTicketDTO> Done { get; set; } = new();
    }
}

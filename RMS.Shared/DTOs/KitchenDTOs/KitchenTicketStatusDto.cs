using RMS.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Shared.DTOs.KitchenDTOs
{
    public class KitchenTicketStatusDto
    {
        public int Id { get; set; }
        public TicketStatus Status { get; set; }

        public DateTime? StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }

        public bool IsOrderReady { get; set; } 
    }
}

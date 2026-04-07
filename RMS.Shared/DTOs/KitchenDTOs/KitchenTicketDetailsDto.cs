using RMS.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Shared.DTOs.KitchenDTOs
{
    public class KitchenTicketDetailsDto
    {
        public int Id { get; set; }
        public string Station { get; set; } = string.Empty;
        public TicketStatus Status { get; set; }

        public DateTime? StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }

        public int OrderId { get; set; }

        public List<string> Items { get; set; } = new();
    }
}

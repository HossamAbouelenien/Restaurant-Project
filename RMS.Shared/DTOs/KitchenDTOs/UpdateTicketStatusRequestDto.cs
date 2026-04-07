using RMS.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Shared.DTOs.KitchenDTOs
{
    public class UpdateTicketStatusRequestDto
    {
        public TicketStatus Status { get; set; }
    }
}

using RMS.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Shared.QueryParams
{
    public class KitchenTicketQueryParams
    {
        public int? branchId { get; set; }
        public int? OrderId { get; set; }
        public string? Station { get; set; } = string.Empty;
        public TicketStatus? Status { get; set; } 
    }
}

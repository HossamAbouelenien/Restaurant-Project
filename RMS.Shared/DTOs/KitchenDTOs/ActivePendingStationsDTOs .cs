using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Shared.DTOs.KitchenDTOs
{
    public class ActivePendingStationsDTOs
    {
        public string? Station { get; set; } = string.Empty;
        public int? PendingCount { get; set; }
    }
}

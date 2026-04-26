using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Shared.DTOs.MenuItemDTOs
{
    public class MenuItemsStatsDTO
    {
        public int TotalItems { get; set; }
        public int AvailableItems { get; set; }
        public int UnavailableItems { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Shared.DTOs.ReportsDTOs
{
    public class DailyRevenueDTO
    {
        public DateTime Date { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}

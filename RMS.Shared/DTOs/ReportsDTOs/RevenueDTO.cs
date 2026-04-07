using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Shared.DTOs.ReportsDTOs
{
    public class RevenueDTO
    {
        public DateTime Date { get; set; }
        public int BranchId { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}

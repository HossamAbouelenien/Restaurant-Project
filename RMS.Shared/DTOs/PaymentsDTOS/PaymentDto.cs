using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Shared.DTOs.PaymentsDTOS
{
    public class PaymentDto
    {
        public int Id { get; set; }
        public int OrderId { get; set; }

        public int BranchId { get; set; }          
        public string BranchName { get; set; } = default!;  

        public string PaymentMethod { get; set; } = null!;
        public string PaymentStatus { get; set; } = null!;

        public decimal PaidAmount { get; set; }
        public DateTime? PaidAt { get; set; }
    }
}

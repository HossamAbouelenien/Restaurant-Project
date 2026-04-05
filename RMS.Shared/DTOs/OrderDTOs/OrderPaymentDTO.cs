using RMS.Domain.Entities;
using RMS.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Shared.DTOs.OrderDTOs
{
    public class OrderPaymentDTO
    {
        public string PaymentMethod { get; set; } = default!;
        public string PaymentStatus { get; set; } = default!;
        public decimal PaidAmount { get; set; }
        public DateTime? PaidAt { get; set; }
    }
}

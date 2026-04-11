using RMS.Shared.DTOs.AddressDTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Shared.DTOs.DeliveryDTOs
{
    public class UnAssignDeliveryDto
    {
        public int DeliveryId { get; set; }
        public string OrderNumber { get; set; } = null!;
        public string BranchName { get; set; } = null!;
        public string CustomerName { get; set; } = null!;
        public int ItemsCount { get; set; }
        public AddressDto DeliveryAddress { get; set; } = default!;
        public DateTime CreatedAt { get; set; }
    }
}

using RMS.Shared.DTOs.AddressDTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Shared.DTOs.DeliveryDTOs
{
    public class AssignDeliveryDto
    {      
            [Required]
            public int OrderId { get; set; }

            [Required]
            public string DriverId { get; set; } = default!;

            [Required]
            public AddressDto DeliveryAddress { get; set; } = default!;
        
    }
}

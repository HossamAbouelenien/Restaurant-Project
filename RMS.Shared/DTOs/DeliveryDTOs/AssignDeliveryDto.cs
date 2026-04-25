using RMS.Shared.DTOs.AddressDTOs;
using System.ComponentModel.DataAnnotations;

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

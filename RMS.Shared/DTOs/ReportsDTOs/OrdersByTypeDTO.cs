using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Shared.DTOs.ReportsDTOs
{
    public class OrdersByTypeDTO
    {
        public int DineInCount { get; set; }
        public int PickupCount { get; set; }
        public int DeliveryCount { get; set; }
    }
}

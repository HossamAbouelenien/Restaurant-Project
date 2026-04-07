using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Shared.DTOs.OrderDTOs
{
    public class AddedItemsDTO
    {
        public int OrderID { get; set; }
        public List<CreateOrderItemDTO> AddedItems { get; set; } = new List<CreateOrderItemDTO>();
    }
}

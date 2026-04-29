using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Domain.Entities.CustomerBasket
{
    public class BasketItem
    {
        public int Id { get; set; } = default!; // Product Id GUID  (كان سترينج وخليته انتجر )
        public string Name { get; set; } = default!;
        public string PictureUrl { get; set; } = default!;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}

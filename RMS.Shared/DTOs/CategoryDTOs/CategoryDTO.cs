using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Shared.DTOs.CategoryDTOs
{
    public class CategoryDTO
    {

        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string ArabicName { get; set; } = default!;
        public int MenuItemsCount { get; set; }

    }
}

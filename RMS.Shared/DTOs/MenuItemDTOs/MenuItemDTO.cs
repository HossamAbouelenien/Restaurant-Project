using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace RMS.Shared.DTOs.MenuItemsDTOs
{
    public class MenuItemDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; } = true;
        public int PrepTimeMinutes { get; set; }
        public string Category { get; set; } = default!;
        public List<string> ImageUrl { get; set; } = new List<string>();
    }
}

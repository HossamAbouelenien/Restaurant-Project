using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Domain.Entities
{
    public class Images : BaseEntity
    {
        public string ImageUrl { get; set; } = default!;
        public int MenuItemId { get; set; }
        public MenuItem MenuItem { get; set; } = default!;
    }
}

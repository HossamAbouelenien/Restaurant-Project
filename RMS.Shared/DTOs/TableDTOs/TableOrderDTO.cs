using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Shared.DTOs.TableDTOs
{
    public class TableOrderDTO
    {
        public int Id { get; set; }
        public int TableId { get; set; }
        public int OrderId { get; set; }
        public DateTime SeatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
    }
}

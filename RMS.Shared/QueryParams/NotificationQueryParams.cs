using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Shared.QueryParams
{
    public class NotificationQueryParams
    {
        public string? UserId { get; set; }
        public string? Role { get; set; }
        public int? BranchId { get; set; }
    }
}

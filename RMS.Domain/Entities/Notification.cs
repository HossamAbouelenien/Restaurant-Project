using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Domain.Entities
{
    public class Notification : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string ArabicTitle { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string ArabicMessage { get; set; } = string.Empty;
        public string Type { get; set; } = "General";
        public bool IsRead { get; set; } = false;
        public string? Role { get; set; }

        #region Relationships 
        public int? BranchId { get; set; }
        public Branch? Branch { get; set; }
        public string? UserId { get; set; }
        public User? User { get; set; }
        #endregion

    }
}

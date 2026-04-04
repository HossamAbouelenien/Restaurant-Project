using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Shared.DTOs.UserDTOs
{
   public class UserDetailsDTO
    {
        
            public string Id { get; set; }

            public string UserName { get; set; }

            public string Email { get; set; }

            public string Name { get; set; }

            public string RoleId { get; set; }
            public DateTime CreatedAt { get; set; }

            public int? BranchId { get; set; }

            public string? BranchName { get; set; } 

            public List<object>? Orders { get; set; }

            public List<object>? Deliveries { get; set; }
        
    }
}

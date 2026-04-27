using RMS.Domain.Entities;
using RMS.Shared.QueryParams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Services.Specifications.UserSpec
{
    public class UserAddressesSpecification : BaseSpecifications<User>
    {
        public UserAddressesSpecification(string userId)
            : base(u => u.Id == userId)
        {
            AddInclude(u => u.Addresses);
        }
    }
}

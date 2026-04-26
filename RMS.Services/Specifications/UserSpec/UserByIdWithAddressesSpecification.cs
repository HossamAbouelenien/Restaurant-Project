using RMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Services.Specifications.UserSpec
{
    public class UserByIdWithAddressesSpecification : BaseSpecifications<User>
    {
        public UserByIdWithAddressesSpecification(string userId)
            : base(u => u.Id == userId)
        {
            AddInclude(u => u.Addresses);
        }
    }
}

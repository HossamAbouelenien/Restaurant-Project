using RMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Services.Specifications.OrderSpec
{
    internal class UserSpecification : BaseSpecifications<User>
    {
            public UserSpecification(string userId) : base(u => u.Id == userId)
            {}
    }
}

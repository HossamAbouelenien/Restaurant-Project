using RMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Services.Specifications.UserSpec
{
    public class UserByIdIgnoreFilterSpecification : BaseSpecifications<User>
    {
        public UserByIdIgnoreFilterSpecification(string id)
             : base(u => u.Id == id)
        {
            IgnoreQueryFilters = true;
        }
    }
}

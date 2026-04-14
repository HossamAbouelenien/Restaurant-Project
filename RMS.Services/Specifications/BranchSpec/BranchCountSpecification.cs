using RMS.Domain.Entities;
using RMS.Shared.QueryParams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Services.Specifications.BranchSpec
{
    public class BranchCountSpecification : BaseSpecifications<Branch>
    {
        public BranchCountSpecification(BranchQueryParams param)
            : base(b => string.IsNullOrEmpty(param.Role)
                || b.Users.Any(u => u.RoleId == param.Role))
        {
        }
    }
}

using RMS.Domain.Entities;
using RMS.Shared.QueryParams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Services.Specifications.UserSpec
{
    public class UsersCountSpecification : BaseSpecifications<User>
    {
        public UsersCountSpecification(UserQueryParams queryParams)
            :base(b => (!queryParams.branchId.HasValue || b.BranchId == queryParams.branchId.Value) &&
                                      (string.IsNullOrEmpty(queryParams.roleId) || b.RoleId == queryParams.roleId))
        {
            
        }
    }
}

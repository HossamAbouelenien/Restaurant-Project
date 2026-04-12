using RMS.Domain.Entities;
using RMS.Shared.QueryParams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Services.Specifications.UserSpec
{
    internal class InactiveUsersCountSpecification : BaseSpecifications<User>
    {
        public InactiveUsersCountSpecification(UserQueryParams queryParams)
            :base(u =>
            (!queryParams.branchId.HasValue || u.BranchId == queryParams.branchId.Value) &&
            (string.IsNullOrEmpty(queryParams.roleId) || u.RoleId == queryParams.roleId) &&
            u.IsDeleted)
        {
            IgnoreQueryFilters = true;

        }
    }
}

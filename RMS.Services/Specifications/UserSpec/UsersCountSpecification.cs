using RMS.Domain.Entities;
using RMS.Shared.QueryParams;

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

using RMS.Domain.Entities;
using RMS.Shared.QueryParams;

namespace RMS.Services.Specifications.UserSpec
{
    public class InactiveUsersWithBranchSpecification : BaseSpecifications<User>
    {
        public InactiveUsersWithBranchSpecification(UserQueryParams queryParams) 
            :base(u =>
                (!queryParams.branchId.HasValue || u.BranchId == queryParams.branchId.Value) &&
                (string.IsNullOrEmpty(queryParams.roleId) || u.RoleId == queryParams.roleId) &&
                u.IsDeleted
            )
        {
            IgnoreQueryFilters = true;

            AddInclude(u => u.Branch!);

            ApplyPagination(queryParams.PageSize, queryParams.PageIndex);

        }
    }
}

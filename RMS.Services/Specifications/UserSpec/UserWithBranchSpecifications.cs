using RMS.Domain.Entities;
using RMS.Shared.QueryParams;

namespace RMS.Services.Specifications.UserSpec
{
    public class UserWithBranchSpecifications :BaseSpecifications<User>
    {
        public UserWithBranchSpecifications(string id) : base(b => b.Id == id)
        {
            AddInclude(b => b.Branch!);
            AddInclude(u => u.Orders);
            AddInclude(u => u.Deliveries);
        }
        public UserWithBranchSpecifications(UserQueryParams queryParams) 
            :base
            (
                 b => (!queryParams.branchId.HasValue || b.BranchId==queryParams.branchId.Value) &&
                 (string.IsNullOrEmpty(queryParams.roleId) || b.RoleId == queryParams.roleId)
            )
        {
            AddInclude(b => b.Branch!);
            ApplyPagination(queryParams.PageSize, queryParams.PageIndex);
        }
    }
}

using RMS.Domain.Entities;
using RMS.Shared.QueryParams;

namespace RMS.Services.Specifications.BranchSpec
{
    public class BranchCountSpecification : BaseSpecifications<Branch>
    {
        public BranchCountSpecification(BranchQueryParams param)
            : base(b =>
      (string.IsNullOrEmpty(param.Role) || b.Users.Any(u => u.RoleId == param.Role))
      &&
      (string.IsNullOrEmpty(param.Search) || b.Name.ToLower().Contains(param.Search.ToLower()))
  )

        {

        }
    }
}

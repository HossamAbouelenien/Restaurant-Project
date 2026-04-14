using RMS.Domain.Entities;
using RMS.Shared.QueryParams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Services.Specifications.BranchSpec
{
    public class BranchWithDetailsSpecification : BaseSpecifications<Branch>
    {
        public BranchWithDetailsSpecification(BranchQueryParams param)
          : base(b => string.IsNullOrEmpty(param.Role)
              || b.Users.Any(u => u.RoleId == param.Role))
        {
            
            AddInclude(b => b.Users);
            AddInclude(b => b.Tables);
            AddInclude(b => b.BranchStocks);

         
            ApplyPagination(param.PageSize, param.PageIndex);

            
            AddOrderBy(b => b.Name);
        }

        public BranchWithDetailsSpecification(int id)
      : base(b => b.Id == id)
        {
            AddInclude(b => b.Users);
            AddInclude(b => b.Tables);
            AddInclude(b => b.BranchStocks);
        }
    }
}


using RMS.Shared.QueryParams;
using RMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Services.Specifications.Tablespec
{
    public class TableSpecification : BaseSpecifications<Table>
    {
       
        public TableSpecification(int id)
            : base(t => t.Id == id )
        {
            AddInclude(t => t.Branch!);
        }

       
        public TableSpecification(TableQueryParams queryParams)
            : base(t =>
                (!queryParams.BranchId.HasValue || t.BranchId == queryParams.BranchId.Value) &&
                (!queryParams.IsOccupied.HasValue || t.IsOccupied == queryParams.IsOccupied.Value) &&
                !t.IsDeleted && (string.IsNullOrEmpty(queryParams.Search) || t.TableNumber.ToLower().Contains(queryParams.Search.ToLower().Trim())))
        {
            AddInclude(t => t.Branch!);
            ApplyPagination(queryParams.PageSize, queryParams.PageIndex);
        }
       
    }
}

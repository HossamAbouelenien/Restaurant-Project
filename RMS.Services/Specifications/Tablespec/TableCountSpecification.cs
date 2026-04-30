using RMS.Domain.Entities;
using RMS.Shared.QueryParams;

namespace RMS.Services.Specifications.Tablespec
{
    public class TableCountSpecification : BaseSpecifications<Table>
    {
        public TableCountSpecification(TableQueryParams queryParams)
            : base(t =>
                    (!queryParams.BranchId.HasValue || t.BranchId == queryParams.BranchId.Value) &&
                    (!queryParams.IsOccupied.HasValue || t.IsOccupied == queryParams.IsOccupied.Value) &&
                    !t.IsDeleted
                    )
        {
            
        }
    }
}

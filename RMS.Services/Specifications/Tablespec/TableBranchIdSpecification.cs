using RMS.Domain.Entities;

namespace RMS.Services.Specifications.Tablespec
{
    internal class TableBranchIdSpecification:BaseSpecifications<Table>
    {
        public TableBranchIdSpecification(int branchId)
           : base(t =>
               (t.BranchId == branchId))


        {

        }
    }
}

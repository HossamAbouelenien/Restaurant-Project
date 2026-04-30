using RMS.Domain.Entities;

namespace RMS.Services.Specifications.Tablespec
{
    public class TableWithTableOrderAndOrderAndBranchSpecification : BaseSpecifications<Table>
     {
        public TableWithTableOrderAndOrderAndBranchSpecification(int id)
                : base(t => t.Id == id)
        {
            AddInclude("TableOrders.Order");
            AddInclude(t => t.Branch!);
        }
    
    }
}

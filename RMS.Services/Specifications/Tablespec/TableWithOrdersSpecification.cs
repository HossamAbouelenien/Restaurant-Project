using RMS.Domain.Entities;

namespace RMS.Services.Specifications.Tablespec
{
   public class TableWithOrdersSpecification :BaseSpecifications<Table>
    {
        public TableWithOrdersSpecification(int id)
         : base(t => t.Id == id && !t.IsDeleted)
        {
            AddInclude("TableOrders.Order");
        }
    }
}



using RMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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



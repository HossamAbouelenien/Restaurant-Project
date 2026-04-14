using Microsoft.EntityFrameworkCore;
using RMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

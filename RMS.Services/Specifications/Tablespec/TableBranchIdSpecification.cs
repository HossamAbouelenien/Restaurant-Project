using RMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

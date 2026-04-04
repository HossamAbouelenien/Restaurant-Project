using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Shared.QueryParams
{
    public class BrandStockQueryParams
    {
        public int? branchId { get; set; }
        public bool? low { get; set; }
    }
}

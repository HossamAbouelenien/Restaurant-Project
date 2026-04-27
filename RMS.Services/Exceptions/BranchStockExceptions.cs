using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RMS.Services.Exceptions.Base;
using RMS.Shared.SharedResources;

namespace RMS.Services.Exceptions
{



    public sealed class BranchStockNotFoundException(int id)
        : NotFoundException(SharedResourcesKeys.BranchStockNotFound, id)
    { }




}


























































































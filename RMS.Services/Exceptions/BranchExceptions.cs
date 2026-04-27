using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RMS.Services.Exceptions.Base;
using RMS.Shared.SharedResources;

namespace RMS.Services.Exceptions
{
    public sealed class BranchNotFoundException(int id)
         : NotFoundException(SharedResourcesKeys.BranchNotFound, id)
    { }

    public sealed class DeleteInActiveBranchException(int id)
        : ConflictException(SharedResourcesKeys.DeleteInActiveBranch, id)
    { }

    public sealed class BranchHasActiveOrdersException(int id)
        : ConflictException(SharedResourcesKeys.DeleteBranchWithActiveOrders, id)
    { }
}
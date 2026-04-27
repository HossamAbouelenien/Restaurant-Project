using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RMS.Services.Exceptions.Base;
using RMS.Shared.SharedResources;

namespace RMS.Services.Exceptions
{
    public sealed class BasketNotFoundException(string basketId) 
        : NotFoundException(SharedResourcesKeys.BasketNotFound, basketId) { }


    public sealed class BasketOperationFailedException(string basketId)
        : BadRequestException(SharedResourcesKeys.BasketOperationFailed, basketId)
    { }



}

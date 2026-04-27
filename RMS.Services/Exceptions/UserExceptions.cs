using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RMS.Services.Exceptions.Base;
using RMS.Shared.SharedResources;

namespace RMS.Services.Exceptions
{




    public sealed class UserNotFoundExceptionId(string id)
          : NotFoundException(SharedResourcesKeys.UserNotFoundId, id)
    { }

    public sealed class UserCreationFailedException(string errors)
        : BadRequestException(SharedResourcesKeys.UserCreationFailed, errors)
    { }

    public sealed class AddressNotFoundException(string userId)
        : NotFoundException(SharedResourcesKeys.AddressNotFound, userId)
    { }






}




































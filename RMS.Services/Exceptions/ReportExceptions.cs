using RMS.Services.Exceptions.Base;
using RMS.Shared.SharedResources;

namespace RMS.Services.Exceptions
{




    public sealed class FailedToRetrieveOrdersException()
         : BadRequestException(SharedResourcesKeys.FailedToRetrieveOrders)
    { }

    public sealed class FailedToRetrieveStockException()
        : BadRequestException(SharedResourcesKeys.FailedToRetrieveStock)
    { }

    public sealed class InvalidTopValueException()
        : BadRequestException(SharedResourcesKeys.InvalidTopValue)
    { }









}
































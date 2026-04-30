using RMS.Services.Exceptions.Base;
using RMS.Shared.SharedResources;

namespace RMS.Services.Exceptions
{
    public sealed class DeliveryNotFoundException(int id)
        : NotFoundException(SharedResourcesKeys.DeliveryNotFound, id)
    { }


    public sealed class OrderNotFoundException(int id)
        : NotFoundException(SharedResourcesKeys.OrderNotFound, id)
    { }


    public sealed class DriverNotFoundException(string driverId)
        : NotFoundException(SharedResourcesKeys.DriverNotFound, driverId)
    { }


    public sealed class InvalidOrderTypeException(string type)
    : BadRequestException(SharedResourcesKeys.InvalidOrderType, type)
    { }


    public sealed class OrderAlreadyAssignedException(int orderId)
        : ConflictException(SharedResourcesKeys.OrderAssignedToDelivery, orderId)
    { }


    public sealed class InvalidDriverRoleException(string driverId)
        : BadRequestException(SharedResourcesKeys.InvalidDriverRole, driverId)
    { }


    public sealed class UnauthorizedDriverException()
        : BadRequestException(SharedResourcesKeys.UnauthorizedDriver)
    { }


    public sealed class InvalidStatusValueException(string status)
        : BadRequestException(SharedResourcesKeys.InvalidStatusValue, status)
    { }


    public sealed class InvalidStatusTransitionException(string current, string next)
        : BadRequestException(SharedResourcesKeys.InvalidStatusTransition, current, next)
    { }


    public sealed class OrderNotReadyException(int orderId)
        : BadRequestException(SharedResourcesKeys.OrderIsNotReadyYet, orderId)
    { }

















}




























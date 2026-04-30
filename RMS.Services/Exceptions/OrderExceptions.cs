using RMS.Services.Exceptions.Base;
using RMS.Shared.SharedResources;

namespace RMS.Services.Exceptions
{
   
    public sealed class OrderItemNotFoundException(int id)
        : NotFoundException(SharedResourcesKeys.OrderItemNotFound, id)
    { }


    public sealed class InvalidPaymentMethodException(string method)
        : BadRequestException(SharedResourcesKeys.InvalidPaymentMethod, method)
    { }

    public sealed class TableRequiredException()
        : ValidationException(SharedResourcesKeys.TableID)
    { }

    public sealed class DeliveryAddressRequiredException()
        : ValidationException(SharedResourcesKeys.DeliveryAddress)
    { }

    public sealed class OrderItemRequiredException()
        : ValidationException(SharedResourcesKeys.OrderItem)
    { }

    public sealed class DuplicateMenuItemsException()
        : ConflictException(SharedResourcesKeys.DuplicateMenuItems)
    { }

    public sealed class InsufficientStockException(string details)
        : BadRequestException(SharedResourcesKeys.InsufficientStock, details)
    { }

    public sealed class TableNotInBranchException()
        : BadRequestException(SharedResourcesKeys.TableNotInBranch)
    { }

    public sealed class TableAlreadyOccupiedOrderException()
        : ConflictException(SharedResourcesKeys.AlreadyOccupiedTable)
    { }

    public sealed class InvalidStatusValueOrderException(string status)
        : BadRequestException(SharedResourcesKeys.InvalidStatusValue, status)
    { }

    public sealed class InvalidStatusTransitionOrderException(string current, string next)
        : BadRequestException(SharedResourcesKeys.InvalidStatusTransition, current, next)
    { }

    public sealed class OrderAlreadyPaidException(int orderId)
        : ConflictException(SharedResourcesKeys.OrderAlreadyPaid, orderId)
    { }

    public sealed class PaymentNotFoundException(int orderId)
        : NotFoundException(SharedResourcesKeys.PaymentNotFound, orderId)
    { }

    public sealed class CancelReceivedOrderOnlyException()
        : BadRequestException(SharedResourcesKeys.CancelReceivedOrderOnly)
    { }

    public sealed class RemoveAllOrderItemsException()
        : BadRequestException(SharedResourcesKeys.RemoveAllOrderItems)
    { }

    public sealed class FailedRetrieveCreatedOrderException()
        : BadRequestException(SharedResourcesKeys.FailedRetrieveCreatedOrder)
    { }

    public sealed class IngredientNotInBranchException(int ingredientId)
        : NotFoundException(SharedResourcesKeys.IngredientNotInBranch, ingredientId)
    { }

    public sealed class FailedUpdatingStatusException()
        : BadRequestException(SharedResourcesKeys.FailedUpdatingStatus)
    { }

    public sealed class DeleteReceivedOrderException()
        : BadRequestException(SharedResourcesKeys.DeleteReceivedOrder)
    { }

    public sealed class TableNotFoundException(int id)
    : NotFoundException(SharedResourcesKeys.NotFound, id)
    { }

    public sealed class UnauthorizedOrderException()
    : BadRequestException(SharedResourcesKeys.UnauthorizedOrder)
    { }





}




























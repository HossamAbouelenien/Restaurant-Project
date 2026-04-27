using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Shared.SharedResources
{
    public static class SharedResourcesKeys
    {
       // General

        public const string Required = "Required";
        public const string Invalid = "Invalid";
        public const string NotFound = "NotFound";
        public const string AlreadyExists = "AlreadyExists";
        public const string Unauthorized = "Unauthorized";
        public const string Failed = "Failed";
        public const string Success = "Success";
        public const string Error = "Error";
        public const string FailedToUpdate = "FailedToUpdate";
        public const string FailedToDelete = "FailedToDelete";




        // Branch
        public const string BranchNotFound = "BranchNotFound";
        public const string DeleteInActiveBranch = "DeleteInActiveBranch";
        public const string DeleteBranchWithActiveOrders = "DeleteBranchWithActiveOrders";



        // BranchStock
        public const string BranchStockNotFound = "BranchStockNotFound";


        // MenuItem
        public const string MenuItemNotFound = "MenuItemNotFound";
        public const string MenuItemNoIngredients = "MenuItemNoIngredients";
        public const string InvalidIngredientIds = "InvalidIngredientIds";
        public const string ImageUploadFailed = "ImageUploadFailed";
        public const string DeleteActiveMenuItem = "DeleteActiveMenuItem";


        // Kitchen
        public const string KitchenTicketNotFound = "KitchenTicketNotFound";



        // Notification
        public const string NotificationNotFound = "NotificationNotFound";


        // Category

        public const string CategoryNotFound = "CategoryNotFound";
        public const string CategoryAlreadyExists = "CategoryAlreadyExists";
        public const string CategoryNameRequired = "CategoryNameRequired";
        public const string DeleteCategoryWithMenuItems = "DeleteCategoryWithMenuItems";




        // Delivery


        public const string InvalidOrderType = "InvalidOrderType";
        public const string OrderAssignedToDelivery = "OrderAssignedToDelivery";
        public const string OrderAssignedToYou = "OrderAssignedToYou";
        public const string InvalidStatusValue = "InvalidStatusValue";
        public const string InvalidStatusTransition = "InvalidStatusTransition";
        public const string OrderIsNotReadyYet = "OrderIsNotReadyYet";

        public const string DeliveryNotFound = "DeliveryNotFound";
        public const string OrderNotFound = "OrderNotFound";
        public const string DriverNotFound = "DriverNotFound";
        public const string InvalidDriverRole = "InvalidDriverRole";
        public const string UnauthorizedDriver = "UnauthorizedDriver";






        // Auth

        public const string EmailIsNotConfirmed = "EmailIsNotConfirmed";
        public const string ConfirmEmail = "ConfirmEmail";
        public const string ErrorHappend = "ErrorHappend";

        public const string EmailNotConfirmed = "EmailNotConfirmed";
        public const string EmailAlreadyExists = "EmailAlreadyExists";
        public const string RegistrationFailed = "RegistrationFailed";
        public const string AuthError = "AuthError";
        public const string UserNotFound = "UserNotFound";



        // MenuItem

        public const string MenuItemIngredient = "MenuItemIngredient";
        public const string DuplicateIngredient = "DuplicateIngredient";



        // Payment

        public const string OrderAlreadyPaid = "OrderAlreadyPaid";
        public const string UnauthorizedOrder = "UnauthorizedOrder";



        // Order
        public const string FailedOrders = "FailedOrders";
        public const string FailedUpdatingStatus = "FailedUpdatingStatus";
        public const string FailedRetrieveCreatedOrder = "FailedRetrieveCreatedOrder";
        public const string DeleteReceivedOrder = "DeleteReceivedOrder";
        public const string RemoveAllOrderItems = "RemoveAllOrderItems";
        public const string DuplicateMenuItems = "DuplicateMenuItems";
        public const string FailedAddingOrder = "FailedAddingOrder";
        public const string TableID = "TableID";
        public const string DeliveryAddress = "DeliveryAddress";
        public const string OrderItem = "OrderItem";

        public const string InvalidPaymentMethod = "InvalidPaymentMethod";
        public const string InsufficientStock = "InsufficientStock";
        public const string TableRequired = "TableRequired";
        public const string DeliveryAddressRequired = "DeliveryAddressRequired";
        public const string OrderItemRequired = "OrderItemRequired";
        public const string PaymentNotFound = "PaymentNotFound";
        public const string CancelReceivedOrderOnly = "CancelReceivedOrderOnly";
        public const string OrderItemNotFound = "OrderItemNotFound";
        public const string IngredientNotInBranch = "IngredientNotInBranch";


        // Table
        public const string TableCapacityInvalid = "TableCapacityInvalid";
        public const string OccupiedTable = "OccupiedTable";
        public const string AlreadyOccupiedTable = "AlreadyOccupiedTable";
        public const string TableBranch = "TableBranch";
        public const string TableNotInBranch = "TableNotInBranch";
        public const string FreeTable = "FreeTable";
        public const string CompletedTableOrder = "CompletedTableOrder";
        public const string TableNumberRequired = "TableNumberRequired";
        public const string TableNumberAlreadyExists = "TableNumberAlreadyExists";
        public const string TableOrderNotFound = "TableOrderNotFound";




        // Basket
        public const string BasketNotFound = "BasketNotFound";
        public const string BasketOperationFailed = "BasketOperationFailed";



        // Ingredient
        public const string IngredientNotFound = "IngredientNotFound";
        public const string IngredientAlreadyExists = "IngredientAlreadyExists";
        public const string IngredientNameRequired = "IngredientNameRequired";


        // Recipe
        public const string RecipeNotFound = "RecipeNotFound";
        public const string RecipeAlreadyExists = "RecipeAlreadyExists";
        public const string RecipeAddFailed = "RecipeAddFailed";
        public const string RecipeUpdateFailed = "RecipeUpdateFailed";
        public const string RecipeDeleteFailed = "RecipeDeleteFailed";



        // Report
        public const string FailedToRetrieveOrders = "FailedToRetrieveOrders";
        public const string FailedToRetrieveStock = "FailedToRetrieveStock";
        public const string InvalidTopValue = "InvalidTopValue";



        // User
        public const string UserNotFoundId = "UserNotFound";
        public const string UserCreationFailed = "UserCreationFailed";
        public const string AddressNotFound = "AddressNotFound";







    }










}














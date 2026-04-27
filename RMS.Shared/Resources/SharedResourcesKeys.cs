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
        public const string DeleteActiveMenuItem = "DeleteActiveMenuItem";




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



        // Table
        public const string TableCapacityInvalid = "TableCapacityInvalid";
        public const string OccupiedTable = "OccupiedTable";
        public const string AlreadyOccupiedTable = "AlreadyOccupiedTable";
        public const string TableBranch = "TableBranch";
        public const string TableNotInBranch = "TableNotInBranch";
        public const string FreeTable = "FreeTable";
        public const string CompletedTableOrder = "CompletedTableOrder";





        // Basket
        public const string BasketNotFound = "BasketNotFound";
        public const string BasketOperationFailed = "BasketOperationFailed";






















    }










}














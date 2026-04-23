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
        public const string DeleteInActiveBranch = "DeleteInActiveBranch";
        public const string DeleteBranchWithActiveOrders = "DeleteBranchWithActiveOrders";


        // MenuItem
        public const string DeleteActiveMenuItem = "DeleteActiveMenuItem";

        // Category

        public const string DeleteCategoryWithMenuItems = "DeleteCategoryWithMenuItems";

        // Delivery
        public const string InvalidOrderType = "InvalidOrderType";
        public const string OrderAssignedToDelivery = "OrderAssignedToDelivery";
        public const string OrderAssignedToYou = "OrderAssignedToYou";
        public const string InvalidStatusValue = "InvalidStatusValue";
        public const string InvalidStatusTransition = "InvalidStatusTransition";


        // Auth
         
        public const string EmailIsNotConfirmed = "EmailIsNotConfirmed";
        public const string ConfirmEmail = "ConfirmEmail";
        public const string ErrorHappend = "ErrorHappend";


        // MenuItem

        public const string MenuItemIngredient = "MenuItemIngredient";
        public const string DuplicateIngredient = "DuplicateIngredient";

        // Payment

        public const string OrderAlreadyPaid = "OrderAlreadyPaid";

        // Order
        public const string FailedOrders = "FailedOrders";

    }
}
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


        // Branch
        public const string DeleteInActiveBranch = "DeleteInActiveBranch";
        public const string DeleteBranchWithActiveOrders = "DeleteBranchWithActiveOrders";


        // MenuItem
        public const string DeleteActiveMenuItem = "DeleteActiveMenuItem";

        // Category

        public const string DeleteCategoryWithMenuItems = "DeleteCategoryWithMenuItems";

    }
}
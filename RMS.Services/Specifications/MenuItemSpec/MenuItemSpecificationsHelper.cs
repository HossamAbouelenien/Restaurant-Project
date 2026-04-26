using RMS.Domain.Entities;
using RMS.Shared.QueryParams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Services.Specifications.MenuItemSpec
{
    internal static class MenuItemSpecificationsHelper 
    {
        public static Expression<Func<MenuItem, bool>> GetMenuItemCriteria(MenuItemQueryParams queryParams)
        {
            return m => (!queryParams.CategoryId.HasValue || m.CategoryId == queryParams.CategoryId)
            && (!queryParams.IsAvailable.HasValue || m.IsAvailable == queryParams.IsAvailable)
            && (!queryParams.BranchId.HasValue || m.Recipes.All(r =>
            r.Ingredient != null &&
            r.Ingredient.BranchStocks.Any(bs =>
                bs.BranchId == queryParams.BranchId &&
                bs.QuantityAvailable >= r.QuantityRequired
            )
            && (string.IsNullOrEmpty(queryParams.Search) || m.Name.ToLower().Contains(queryParams.Search.ToLower().Trim()))));
        }

    }
}

using RMS.Domain.Entities;
using RMS.Shared.QueryParams;
using System.Linq.Expressions;

namespace RMS.Services.Specifications.MenuItemSpec
{
    internal static class MenuItemSpecificationsHelper 
    {
        public static Expression<Func<MenuItem, bool>> GetMenuItemCriteria(MenuItemQueryParams queryParams)
        {
            return m =>
                    (!queryParams.CategoryId.HasValue || m.CategoryId == queryParams.CategoryId)
                 && (!queryParams.IsAvailable.HasValue || m.IsAvailable == queryParams.IsAvailable)
                 && (!queryParams.BranchId.HasValue || m.Recipes.All(r =>
                                r.Ingredient != null &&
                                r.Ingredient.BranchStocks.Any(bs =>
                                bs.BranchId == queryParams.BranchId &&
                                bs.QuantityAvailable >= r.QuantityRequired
                    )))
                 && (
                 string.IsNullOrEmpty(queryParams.Search) ||
                 m.Name.ToLower().Contains(queryParams.Search.ToLower().Trim()) ||
                 m.ArabicName.ToLower().Contains(queryParams.Search.Trim())
           );
        }

    }
}

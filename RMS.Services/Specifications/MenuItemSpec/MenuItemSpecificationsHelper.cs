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
            && (string.IsNullOrEmpty(queryParams.Search) || m.Name.ToLower().Contains(queryParams.Search.ToLower()));
        }

    }
}

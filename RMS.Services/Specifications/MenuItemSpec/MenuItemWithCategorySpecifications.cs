using RMS.Domain.Entities;
using RMS.Shared.QueryParams;
using RMS.Shared.SortingOptionsEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Services.Specifications.MenuItemSpec
{
    public class MenuItemWithCategorySpecifications : BaseSpecifications<MenuItem>
    {
        public MenuItemWithCategorySpecifications(MenuItemQueryParams queryParams) 
            : base(MenuItemSpecificationsHelper.GetMenuItemCriteria(queryParams))
        {
            //Include the category information when retrieving menu items
            AddInclude(m => m.Category!);
            //AddInclude("Recipes.Ingredient.BranchStocks");
            // Apply sorting based on the specified sorting option in the query parameters
            switch (queryParams.Sort)
            {
                
                case MenuItemSortingOptions.PriceAsc:
                    AddOrderBy(m => m.Price);
                    break;
                case MenuItemSortingOptions.PriceDesc:
                    AddOrderByDescending(m => m.Price);
                    break;
                default:
                    AddOrderBy(m => m.Id);
                    break;
            }

            // Apply pagination based on the specified page size and page index in the query parameters
            ApplyPagination(queryParams.PageSize, queryParams.PageIndex);
        }
    }
}

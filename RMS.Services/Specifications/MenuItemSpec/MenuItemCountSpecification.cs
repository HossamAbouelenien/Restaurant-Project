using RMS.Domain.Entities;
using RMS.Shared.QueryParams;

namespace RMS.Services.Specifications.MenuItemSpec
{
    internal class MenuItemCountSpecification : BaseSpecifications<MenuItem>
    {
        public MenuItemCountSpecification(MenuItemQueryParams queryParams)
            : base(MenuItemSpecificationsHelper.GetMenuItemCriteria(queryParams))
        {
            
        }
    }
}

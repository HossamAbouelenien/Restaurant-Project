using RMS.Domain.Entities;
using RMS.Shared.QueryParams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

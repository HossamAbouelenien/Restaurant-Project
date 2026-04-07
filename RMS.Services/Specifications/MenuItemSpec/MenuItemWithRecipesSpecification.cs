using RMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Services.Specifications.MenuItemSpec
{
    internal class MenuItemWithRecipesSpecification : BaseSpecifications<MenuItem>
    {
        public MenuItemWithRecipesSpecification(int menuItemId) : base(m => m.Id  == menuItemId )
        {
            AddInclude(m => m.Recipes);
        }
    }
}

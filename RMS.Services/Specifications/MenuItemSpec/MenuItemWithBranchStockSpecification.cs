using RMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Services.Specifications.MenuItemSpec
{
    internal class MenuItemWithBranchStockSpecification :BaseSpecifications<MenuItem>
    {
        public MenuItemWithBranchStockSpecification(int id) : base(m => m.Id == id)
        {
            AddInclude("Recipes.Ingredient.BranchStocks");
            AddInclude(m => m.Category!);
        }
    }
}

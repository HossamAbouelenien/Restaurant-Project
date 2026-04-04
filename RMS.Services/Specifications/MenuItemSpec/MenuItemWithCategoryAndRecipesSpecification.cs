using RMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Services.Specifications.MenuItemSpec
{
    public class MenuItemWithCategoryAndRecipesSpecification :BaseSpecifications<MenuItem>
    {
        public MenuItemWithCategoryAndRecipesSpecification(int id) : base(m => m.Id == id)
        {
            AddInclude(m => m.Category!);
            // Include Recipes then ThenInclude Ingredient using dot notation
            AddInclude("Recipes.Ingredient");

        }
    }
}

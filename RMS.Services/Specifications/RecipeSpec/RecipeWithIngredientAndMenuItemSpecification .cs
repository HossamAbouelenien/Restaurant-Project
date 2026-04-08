using RMS.Domain.Entities;
using RMS.Shared.QueryParams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Services.Specifications.RecipeSpec
{
    internal class RecipeWithIngredientAndMenuItemSpecification : BaseSpecifications<Recipe>
    {
        public RecipeWithIngredientAndMenuItemSpecification(RecipesQueryParams queryParams) 
            : base(r => (!queryParams.MenuItemId.HasValue || r.MenuItemId == queryParams.MenuItemId))
        {
            AddInclude(r => r.Ingredient!);
            AddInclude(r => r.MenuItem!);
            
        }
        public RecipeWithIngredientAndMenuItemSpecification(int recipeId)
            :base(r => r.Id == recipeId)
        {
            AddInclude(r => r.Ingredient!);
            AddInclude(r => r.MenuItem!);
        }
    }
}

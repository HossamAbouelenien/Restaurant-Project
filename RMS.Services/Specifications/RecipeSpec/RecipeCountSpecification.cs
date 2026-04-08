using RMS.Domain.Entities;
using RMS.Shared.QueryParams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Services.Specifications.RecipeSpec
{
    public class RecipeCountSpecification : BaseSpecifications<Recipe>
    {
        public RecipeCountSpecification(RecipesQueryParams queryParams)
            :base(r => (!queryParams.MenuItemId.HasValue || r.MenuItemId == queryParams.MenuItemId))
        {
            
        }
    }
}

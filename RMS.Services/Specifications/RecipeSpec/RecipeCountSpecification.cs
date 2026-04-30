using RMS.Domain.Entities;
using RMS.Shared.QueryParams;

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

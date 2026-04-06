using RMS.Domain.Entities;
using RMS.Shared.QueryParams;

namespace RMS.Services.Specifications.BranchStockSpec
{
    public class BranchStockWithBranchAndIngredient :BaseSpecifications<BranchStock>
    {
        public BranchStockWithBranchAndIngredient(int id) : base(b => b.Id == id)
        {
            AddInclude(b => b.Branch!);
            AddInclude(b => b.Ingredient!);
        }
        public BranchStockWithBranchAndIngredient(BrandStockQueryParams queryParams) 
            :base
            (
                 b => (!queryParams.branchId.HasValue || b.BranchId==queryParams.branchId.Value) &&
                 (!queryParams.low.HasValue || !queryParams.low.Value || b.QuantityAvailable<b.LowThreshold)
            )
        {
            AddInclude(b => b.Branch!);
            AddInclude(b => b.Ingredient!);
        }
        public BranchStockWithBranchAndIngredient(int branchId,int ingredientId) : base(b => b.BranchId == branchId && b.IngredientId == ingredientId)
        {
            
        }
    }
}

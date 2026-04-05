
    using RMS.Domain.Entities;

    namespace RMS.Services.Specifications.StockSpec
    {
        public class StockByBranchAndIngredientsSpecification : BaseSpecifications<BranchStock>
        {
            public StockByBranchAndIngredientsSpecification(int branchId, List<int> ingredientIds)
                : base(s =>s.BranchId == branchId && ingredientIds.Contains(s.IngredientId))
                    
                    
            {
            }
        }
    }


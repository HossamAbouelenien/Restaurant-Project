using RMS.Domain.Entities;

namespace RMS.Domain.Contracts
{
    public interface INutritionRepository
    {
        Task<List<MenuItem>> GetMenuItemsWithIngredientsAsync(List<int> menuItemIds, CancellationToken cancellationToken = default);
    }
}

using RMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Domain.Contracts
{
    public interface INutritionRepository
    {
        Task<List<MenuItem>> GetMenuItemsWithIngredientsAsync(List<int> menuItemIds, CancellationToken cancellationToken = default);
    }
}

using Microsoft.EntityFrameworkCore;
using RMS.Domain.Contracts;
using RMS.Domain.Entities;
using RMS.Persistence.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Persistence.Repositries
{
    public class NutritionRepository : INutritionRepository
    {
        private readonly AppDbContext _dbContext;
        public NutritionRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        // Add to your existing MenuItemRepository:
       
        public async Task<List<MenuItem>> GetMenuItemsWithIngredientsAsync(List<int> menuItemIds, CancellationToken cancellationToken = default)
        {
            return await _dbContext.MenuItems
                .Where(m => menuItemIds.Contains(m.Id))
                .Include(m => m.Recipes)
                    .ThenInclude(r => r.Ingredient)
                .AsNoTracking() // read-only, avoids tracking overhead
                .ToListAsync(cancellationToken);
        }
    }
}

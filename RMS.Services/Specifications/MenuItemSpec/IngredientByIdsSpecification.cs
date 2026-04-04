using RMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Services.Specifications.MenuItemSpec
{
    public class IngredientByIdsSpecification : BaseSpecifications<Ingredient>
    {
        public IngredientByIdsSpecification(List<int> ids): base(i => ids.Contains(i.Id))
        {
        }
    }
}

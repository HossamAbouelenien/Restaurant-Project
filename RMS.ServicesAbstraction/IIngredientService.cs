using RMS.Shared.DTOs.IngredientDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.ServicesAbstraction
{
    public interface IIngredientService
    {
        Task<IEnumerable<IngredientDTO>> GetAllIngredientsAsync();
    }
}

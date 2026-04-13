using RMS.Shared;
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
        Task<PaginatedResult<IngredientDTO>> GetAllIngredientsAsync(int pageIndex, int pageSize);
        Task<IngredientDTO> GetIngredientByIdAsync(int id);

        Task<IngredientDTO> CreateIngredientAsync(CreateIngredientDTO createIngredientDTO);

        Task<IngredientDTO> UpdateIngredientAsync(int id, CreateIngredientDTO updateIngredientDTO);

        Task DeleteIngredientAsync(int id);



    }
}

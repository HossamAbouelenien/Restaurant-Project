using AutoMapper;
using RMS.Domain.Contracts;
using RMS.Domain.Entities;
using RMS.ServicesAbstraction;
using RMS.Shared.DTOs.BranchStockDTOs;
using RMS.Shared.DTOs.IngredientDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Services.IngredientServices
{
    public class IngredientService : IIngredientService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public IngredientService(IUnitOfWork unitOfWork , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // Get all Ingredients
        public async Task<IEnumerable<IngredientDTO>> GetAllIngredientsAsync()
        {
            var ingredients = await _unitOfWork.GetRepository<Ingredient>().GetAllAsync();
            var result = _mapper.Map<IEnumerable<IngredientDTO>>(ingredients);
            return result;
        }

        public async Task<IngredientDTO> GetIngredientByIdAsync(int id)
        {
            var ingerdient = await _unitOfWork.GetRepository<Ingredient>().GetByIdAsync(id);
            var result = _mapper.Map<IngredientDTO>(ingerdient);
            return result;
        }

        public async Task<IngredientDTO> CreateIngredientAsync(CreateIngredientDTO dto)
        {
            var repo =  _unitOfWork.GetRepository<Ingredient>();

            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new Exception("Ingredient name is required");

            var existing = await repo.GetAllAsync();
            if (existing.Any(i => i.Name.ToLower() == dto.Name.ToLower()))
                throw new Exception("Ingredient already exists");

            var ingredient = _mapper.Map<Ingredient>(dto);
            await repo.AddAsync(ingredient);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<IngredientDTO>(ingredient);
        }

        public async Task DeleteIngredientAsync(int id)
        {
            var repo = _unitOfWork.GetRepository<Ingredient>();
            var ingredient = await repo.GetByIdAsync(id);
            if (ingredient is null) return;
            ingredient.IsDeleted = true;
            ingredient.DeletedAt = DateTime.UtcNow;
            repo.Update(ingredient);
            await _unitOfWork.SaveChangesAsync();

        }



        public async Task<IngredientDTO> UpdateIngredientAsync(int id, CreateIngredientDTO updateIngredientDTO)
        {
            var repo = _unitOfWork.GetRepository<Ingredient>();
            var ingredient = await repo.GetByIdAsync(id);
            _mapper.Map(updateIngredientDTO, ingredient);

            //if (string.IsNullOrWhiteSpace(updateIngredientDTO.Name))
            //    throw new Exception("Ingredient name is required");

            //var existing = await repo.GetAllAsync();
            //if (existing.Any(i => i.Name.ToLower() == updateIngredientDTO.Name.ToLower()))
            //    throw new Exception("Ingredient already exists");

            repo.Update(ingredient!);
            ingredient.UpdatedAt = DateTime.UtcNow;
            await _unitOfWork.SaveChangesAsync();
            
            var res = _mapper.Map<IngredientDTO>(ingredient);

            return res;
             


        }


    }
}

using AutoMapper;
using RMS.Domain.Contracts;
using RMS.Domain.Entities;
using RMS.Services.Specifications.RecipeSpec;
using RMS.ServicesAbstraction;
using RMS.Shared;
using RMS.Shared.DTOs.MenuItemsDTOs;
using RMS.Shared.DTOs.RecipeDTOs;
using RMS.Shared.QueryParams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Services.RecipeServices
{
    public class RecipeService : IRecipeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RecipeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        public async Task<PaginatedResult<RecipesListDTO>>GetAllRecipesAsync(RecipesQueryParams queryParams)
        {
        var repo =  _unitOfWork.GetRepository<Recipe>();
        var spec = new RecipeWithIngredientAndMenuItemSpecification(queryParams);
        var recipes = await repo.GetAllAsync(spec);
        var recipesToDto = _mapper.Map<IEnumerable<RecipesListDTO>>(recipes);
        var recipesToDtoCount = recipesToDto.Count();
        var countSpec = new RecipeCountSpecification(queryParams);
        var recipesCount = await repo.CountAsync(countSpec);
        var paginatedResult = new PaginatedResult<RecipesListDTO>(queryParams.PageIndex, recipesToDtoCount, recipesCount, recipesToDto);
            return paginatedResult;
        }

        public async Task<RecipesListDTO> AddRecipeToMenuItemAsync(AddRecipeToMenuItemDTO dto)
        {
            var repo = _unitOfWork.GetRepository<Recipe>();
            var menuItem = await _unitOfWork.GetRepository<MenuItem>().GetByIdAsync(dto.MenuItemId);
            if (menuItem == null)
                throw new Exception("Menu Item Not Found");

            var ingredient = await _unitOfWork.GetRepository<Ingredient>().GetByIdAsync(dto.IngredientId);
            if (ingredient == null)
                throw new Exception("Ingredient Not Found");

            var existingRecipes = await repo.GetAllAsync();
            var recipeExists = existingRecipes.Any(r => r.MenuItemId == dto.MenuItemId && r.IngredientId == dto.IngredientId);
            if (recipeExists)
                throw new Exception("This ingredient already exists in the recipe.");

            var recipe = _mapper.Map<Recipe>(dto);
            await repo.AddAsync(recipe);
            var result = await _unitOfWork.SaveChangesAsync();
            if (result > 0)
            {
                var spec = new RecipeWithIngredientAndMenuItemSpecification(recipe.Id);
                var recipeToReturn = repo.GetByIdAsync(spec);
                return _mapper.Map<RecipesListDTO>(recipe);
            } 
            else
                throw new Exception("Failed To Create Recipe");
        }

        public async Task<RecipesListDTO> UpdateRecipeQuantityRequiredAsync(int recipeId, UpdateRecipeQuantityDTO dto)
        {
            var repo = _unitOfWork.GetRepository<Recipe>();
            var recipe = await repo.GetByIdAsync(recipeId);
            if (recipe is null) throw new Exception("Recipe Not Found");

            recipe.QuantityRequired = dto.QuantityRequired;
            recipe.UpdatedAt = DateTime.Now;
            repo.Update(recipe);
            var result = await _unitOfWork.SaveChangesAsync();

            if(result > 0)
            {
                var spec = new RecipeWithIngredientAndMenuItemSpecification(recipeId);
                var updatedRecipe = await repo.GetByIdAsync(spec);
                return _mapper.Map<RecipesListDTO>(updatedRecipe);
            }
            else
                throw new Exception("Failed To Update Recipe");
        }

        public async Task DeleteRecipeAsync(int id)
        {
            var recipe = await _unitOfWork.GetRepository<Recipe>().GetByIdAsync(id);
            if (recipe is null) throw new Exception("Recipe Not Found");

            recipe.IsDeleted = true;
            recipe.DeletedAt = DateTime.Now;

            var result = await _unitOfWork.SaveChangesAsync();
            if (result == 0) throw new Exception("Failed To Delete Recipe");
        }
    }
}

using AutoMapper;
using RMS.Domain.Contracts;
using RMS.Domain.Entities;
using RMS.Services.Exceptions;
using RMS.Services.Specifications.RecipeSpec;
using RMS.ServicesAbstraction;
using RMS.Shared;
using RMS.Shared.DTOs.RecipeDTOs;
using RMS.Shared.QueryParams;

namespace RMS.Services.Services.RecipeServices
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
                throw new MenuItemNotFoundException(dto.MenuItemId);

            var ingredient = await _unitOfWork.GetRepository<Ingredient>().GetByIdAsync(dto.IngredientId);
            if (ingredient == null)
                throw new IngredientNotFoundException(dto.IngredientId);

            var existingRecipes = await repo.GetAllAsync();
            var recipeExists = existingRecipes.Any(r => r.MenuItemId == dto.MenuItemId && r.IngredientId == dto.IngredientId);
            if (recipeExists)
                throw new RecipeAlreadyExistsException(dto.MenuItemId, dto.IngredientId);

            var recipe = _mapper.Map<Recipe>(dto);

            await repo.AddAsync(recipe);

            var result = await _unitOfWork.SaveChangesAsync();
            if (result > 0)
            {
                var spec = new RecipeWithIngredientAndMenuItemSpecification(recipe.Id);
                var recipeToReturn = await repo.GetByIdAsync(spec);
                return _mapper.Map<RecipesListDTO>(recipe);
            } 
            else
                throw new RecipeAddFailedException();
        }

        public async Task<RecipesListDTO> UpdateRecipeQuantityRequiredAsync(int recipeId, UpdateRecipeQuantityDTO dto)
        {
            var repo = _unitOfWork.GetRepository<Recipe>();
            var recipe = await repo.GetByIdAsync(recipeId);
            if (recipe is null) throw new RecipeNotFoundException(recipeId);

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
                throw new RecipeUpdateFailedException(recipeId);
        }

        public async Task DeleteRecipeAsync(int id)
        {
            var recipe = await _unitOfWork.GetRepository<Recipe>().GetByIdAsync(id);
            if (recipe is null) throw new RecipeNotFoundException(id);

            recipe.IsDeleted = true;
            recipe.DeletedAt = DateTime.UtcNow;



            var result = await _unitOfWork.SaveChangesAsync();
            if (result == 0) throw new RecipeDeleteFailedException(id);
        }
    }
}

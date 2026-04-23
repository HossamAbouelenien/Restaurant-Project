using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RMS.Domain.Contracts;
using RMS.Domain.Entities;
using RMS.Services.Specifications.MenuItemSpec;
using RMS.ServicesAbstraction;
using RMS.Shared;
using RMS.Shared.DTOs.MenuItemDTOs;
using RMS.Shared.DTOs.MenuItemsDTOs;
using RMS.Shared.DTOs.RecipeDTOs;
using RMS.Shared.QueryParams;
using RMS.Shared.SharedResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RMS.Services.MenuItemsServices
{
    public class MenuItemService : IMenuItemService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;

        public MenuItemService(IUnitOfWork unitOfWork, IMapper mapper, IImageService imageService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _imageService = imageService;
        }
        public async Task<PaginatedResult<MenuItemDTO>> GetAllMenuItemsAsync(MenuItemQueryParams queryParams)
        {
            var repo = _unitOfWork.GetRepository<MenuItem>();
            var spec = new MenuItemWithCategorySpecifications(queryParams);
            var menuItems = await repo.GetAllAsync(spec);
            //var menuItemsExistsInBranch = menuItems.Where(m => m.Recipes.Any(r => r.Ingredient!.BranchStocks.Any(bs => bs.BranchId == queryParams.BranchId)));
            //var menuItemExistsInBranch = new List<MenuItem>();
            //foreach (var menuItem in menuItems)
            //{
            //    bool isAvailableInBranch = true;
            //    foreach (var recipe in menuItem.Recipes)
            //    {
            //        var ingredient = recipe.Ingredient;
            //        if (ingredient != null && queryParams.BranchId != 0 )
            //        {
            //            var branchStock = ingredient.BranchStocks.FirstOrDefault(bs => bs.BranchId == queryParams.BranchId);
            //            if (branchStock == null || branchStock.QuantityAvailable < recipe.QuantityRequired)
            //            {
            //                isAvailableInBranch = false;
            //                break;
            //            }
            //        }
            //    }
            //    if (isAvailableInBranch)
            //    {
            //        menuItemExistsInBranch.Add(menuItem);
            //    }
            //}
            var returnedMenuItems = _mapper.Map<IEnumerable<MenuItemDTO>>(menuItems);
            var countOfReturnedMenuItems = returnedMenuItems.Count();
            var countSpec = new MenuItemCountSpecification(queryParams);
            var countOfAllMenuItems = await repo.CountAsync(countSpec);
            var paginatedResult = new PaginatedResult<MenuItemDTO>(queryParams.PageIndex, countOfReturnedMenuItems, countOfAllMenuItems, returnedMenuItems);
            return paginatedResult;
        }

        public async Task<MenuItemDetailsDTO?> GetMenuItemByIdAsync(int id)
        {
            var spec = new MenuItemWithCategoryAndRecipesSpecification(id);
            var menuItem = await _unitOfWork.GetRepository<MenuItem>().GetByIdAsync(spec);
            if (menuItem is null) return null;
            return _mapper.Map<MenuItemDetailsDTO>(menuItem);
        }

        public async Task<MenuItemDetailsDTO> CreateMenuItemAsync(CreateMenuItemDTO menuItemDto)
        {
            var repo = _unitOfWork.GetRepository<MenuItem>();
            // Validation
            if (menuItemDto.Recipes == null || !menuItemDto.Recipes.Any())
                throw new Exception(SharedResourcesKeys.MenuItemIngredient);
            // Check for duplicate ingredients
            var duplicates = menuItemDto.Recipes.GroupBy(r => r.IngredientId).Where(g => g.Count() > 1);
            if (duplicates.Any())
                throw new Exception(SharedResourcesKeys.DuplicateIngredient);

            var ingredientIds = menuItemDto.Recipes.Select(r => r.IngredientId).Distinct().ToHashSet();
            var spec = new IngredientByIdsSpecification(ingredientIds);
            var existingIngredients = await _unitOfWork.GetRepository<Ingredient>().GetAllAsync(spec);
            var existingIds = existingIngredients.Select(i => i.Id).ToHashSet();
            var invalidIds = ingredientIds.Except(existingIds);
            if (invalidIds.Any())
                throw new Exception($"Invalid IngredientIds: {string.Join(",", invalidIds)}");

            // Image upload using IImageService
            var imagePath = await _imageService.UploadImageAsync(menuItemDto.Image);
            var menuItem = _mapper.Map<MenuItem>(menuItemDto);
            menuItem.ImageUrl = imagePath;
            menuItem.Recipes = menuItemDto.Recipes.Select(r => new Recipe
            {
                IngredientId = r.IngredientId,
                QuantityRequired = r.QuantityRequired
            }).ToList();

            await repo.AddAsync(menuItem);
            await _unitOfWork.SaveChangesAsync();

            var menuItemWithIdSpec = new MenuItemWithCategoryAndRecipesSpecification(menuItem.Id);
            var created = await repo.GetByIdAsync(menuItemWithIdSpec);
            return _mapper.Map<MenuItemDetailsDTO>(created!);
        }

       
        public async Task<MenuItemDetailsDTO> UpdateMenuItemAsync(int id, UpdateMenuItemDTO menuItemDto)
        {
            var repo = _unitOfWork.GetRepository<MenuItem>();
            //  Check exists
            var spec = new MenuItemWithCategoryAndRecipesSpecification(id);
            var menuItem = await repo.GetByIdAsync(spec);
            if (menuItem is null) throw new Exception(SharedResourcesKeys.NotFound);
            //  Validation
            if (menuItemDto.Recipes == null || !menuItemDto.Recipes.Any())
                throw new Exception(SharedResourcesKeys.MenuItemIngredient);
            // Check for duplicate ingredients
            var duplicates = menuItemDto.Recipes.GroupBy(r => r.IngredientId).Where(g => g.Count() > 1);
            if (duplicates.Any())
                throw new Exception(SharedResourcesKeys.DuplicateIngredient);
            //  Validate Ingredients
            var ingredientIds = menuItemDto.Recipes.Select(r => r.IngredientId).Distinct().ToHashSet();

            var ingredientSpec = new IngredientByIdsSpecification(ingredientIds);

            var existingIngredients = await _unitOfWork.GetRepository<Ingredient>().GetAllAsync(ingredientSpec);
            var existingIds = existingIngredients.Select(i => i.Id).ToHashSet();
            var invalidIds = ingredientIds.Except(existingIds);
            if (invalidIds.Any())
                throw new Exception($"Invalid IngredientIds: {string.Join(",", invalidIds)}");

            if (menuItemDto.Image != null)
            {
                // Replace Old Image
                menuItem.ImageUrl = await _imageService.ReplaceImageAsync(menuItemDto.Image, menuItem.ImageUrl);
            }
            //  Map updated fields ignoreing Recipes and ImageUrl as they are handled separately
            _mapper.Map(menuItemDto, menuItem);

            var existingRecipes = menuItem.Recipes.ToList();
            var incomingRecipes = menuItemDto.Recipes;

            // Update
            foreach (var existingRecipe in existingRecipes)
            {
                var updatedRecipe = incomingRecipes.FirstOrDefault(r => r.IngredientId == existingRecipe.IngredientId);

                if (updatedRecipe != null)
                    existingRecipe.QuantityRequired = updatedRecipe.QuantityRequired;
            }

            // Remove
            foreach (var existingRecipe in existingRecipes)
            {
                if (!incomingRecipes.Any(r => r.IngredientId == existingRecipe.IngredientId))
                    menuItem.Recipes.Remove(existingRecipe);
            }

            // Add
            foreach (var incoming in incomingRecipes)
            {
                if (!existingRecipes.Any(r => r.IngredientId == incoming.IngredientId))
                {
                    menuItem.Recipes.Add(new Recipe
                    {
                        IngredientId = incoming.IngredientId,
                        QuantityRequired = incoming.QuantityRequired
                    });
                }
            }

            await _unitOfWork.SaveChangesAsync();

            //  Return updated
            var specWithIncludes = new MenuItemWithCategoryAndRecipesSpecification(id);

            var updated = await repo.GetByIdAsync(specWithIncludes);

            return _mapper.Map<MenuItemDetailsDTO>(updated!);
        }

        public async Task ToggleAvailabilityAsync(int id)
        {
            var menuItem = await _unitOfWork.GetRepository<MenuItem>().GetByIdAsync(id);
            if (menuItem is null) throw new Exception(SharedResourcesKeys.NotFound);

            menuItem.IsAvailable = !menuItem.IsAvailable;

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteMenuItemAsync(int id)
        {
            var menuItemWithRecipesSpec = new MenuItemWithRecipesSpecification(id);
            var menuItem = await _unitOfWork.GetRepository<MenuItem>().GetByIdAsync(menuItemWithRecipesSpec);
            if (menuItem is null) throw new Exception(SharedResourcesKeys.NotFound);

            foreach (var recipe in menuItem.Recipes)
            {
                _unitOfWork.GetRepository<Recipe>().Remove(recipe);
            }
            menuItem.IsDeleted = true;
            menuItem.DeletedAt = DateTime.Now;

            await _unitOfWork.SaveChangesAsync();
        }


        public async Task<IEnumerable<MenuItemDTO>> GetPopularMenuItemsAsync(int limit)
        {
            var repo = _unitOfWork.GetRepository<MenuItem>();

            var spec = new PopularMenuItemsSpecification(limit);

            var items = await repo.GetAllAsync(spec);

            return _mapper.Map<IEnumerable<MenuItemDTO>>(items);
        }
    }
}

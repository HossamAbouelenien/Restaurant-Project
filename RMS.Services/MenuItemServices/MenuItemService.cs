using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RMS.Domain.Contracts;
using RMS.Domain.Entities;
using RMS.Services.Specifications.MenuItemSpec;
using RMS.ServicesAbstraction;
using RMS.Shared;
using RMS.Shared.DTOs.MenuItemDTOs;
using RMS.Shared.DTOs.MenuItemsDTOs;
using RMS.Shared.QueryParams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Services.MenuItemsServices
{
    public class MenuItemService : IMenuItemService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MenuItemService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<PaginatedResult<MenuItemDTO>> GetAllMenuItemsAsync(MenuItemQueryParams queryParams)
        {
            var repo = _unitOfWork.GetRepository<MenuItem>();
            var spec = new MenuItemWithCategorySpecifications(queryParams);
            var menuItems = await repo.GetAllAsync(spec);
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

        public async Task<MenuItemDetailsDTO> CreateMenuItemAsync(CreateMenuItemDTO dto)
        {
            var repo = _unitOfWork.GetRepository<MenuItem>();
            // Validation
            if (dto.Recipes == null || !dto.Recipes.Any())
                throw new Exception("MenuItem must have at least one ingredient");
            // Check for duplicate ingredients
            var duplicates = dto.Recipes.GroupBy(r => r.IngredientId).Where(g => g.Count() > 1);
            if (duplicates.Any())
                throw new Exception("Duplicate ingredients not allowed");

            var ingredientIds = dto.Recipes.Select(r => r.IngredientId).Distinct().ToList();
            var spec = new IngredientByIdsSpecification(ingredientIds);
            var existingIngredients = await _unitOfWork .GetRepository<Ingredient>().GetAllAsync(spec);
            var existingIds = existingIngredients.Select(i => i.Id).ToList();
            var invalidIds = ingredientIds.Except(existingIds);
            if (invalidIds.Any())
                throw new Exception($"Invalid IngredientIds: {string.Join(",", invalidIds)}");

            // Handle image upload
            string? imagePath = null;
            if (dto.Image != null)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(dto.Image.FileName);

                var filePath = Path.Combine("wwwroot/Images", fileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                await dto.Image.CopyToAsync(stream);

                imagePath = $"Images/{fileName}";
            }
            var menuItem = _mapper.Map<MenuItem>(dto);
            menuItem.ImageUrl = imagePath;
            menuItem.Recipes = dto.Recipes.Select(r => new Recipe
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

        public async Task<MenuItemDetailsDTO> UpdateMenuItemAsync(int id, UpdateMenuItemDTO dto)
        {
            var repo = _unitOfWork.GetRepository<MenuItem>();
            //  Check exists
            var spec = new MenuItemWithCategoryAndRecipesSpecification(id);
            var menuItem = await repo.GetByIdAsync(spec);
            if (menuItem is null) throw new Exception("MenuItem not found");
            //  Validation
            if (dto.Recipes == null || !dto.Recipes.Any())
                throw new Exception("MenuItem must have at least one ingredient");
            // Check for duplicate ingredients
            var duplicates = dto.Recipes.GroupBy(r => r.IngredientId).Where(g => g.Count() > 1);
            if (duplicates.Any())
                throw new Exception("Duplicate ingredients not allowed");
            //  Validate Ingredients
            var ingredientIds = dto.Recipes.Select(r => r.IngredientId).Distinct().ToList();

            var ingredientSpec = new IngredientByIdsSpecification(ingredientIds);

            var existingIngredients = await _unitOfWork
                .GetRepository<Ingredient>()
                .GetAllAsync(ingredientSpec);
            var existingIds = existingIngredients.Select(i => i.Id).ToList();

            var invalidIds = ingredientIds.Except(existingIds);

            if (invalidIds.Any())
                throw new Exception($"Invalid IngredientIds: {string.Join(",", invalidIds)}");

            
            if (dto.Image != null)
            {
                // delete old image
                if (!string.IsNullOrEmpty(menuItem.ImageUrl))
                {
                    var oldPath = Path.Combine("wwwroot", menuItem.ImageUrl);
                    if (File.Exists(oldPath))
                        File.Delete(oldPath);
                }

                var fileName = Guid.NewGuid() + Path.GetExtension(dto.Image.FileName);
                var filePath = Path.Combine("wwwroot/Images", fileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                await dto.Image.CopyToAsync(stream);

                menuItem.ImageUrl = $"Images/{fileName}";
            }
            //  Map updated fields ignoreing Recipes and ImageUrl as they are handled separately
            _mapper.Map(dto, menuItem);

            var existingRecipes = menuItem.Recipes.ToList();
            var incomingRecipes = dto.Recipes;

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
            if (menuItem is null) throw new Exception("MenuItem not found");

            menuItem.IsAvailable = !menuItem.IsAvailable;

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteMenuItemAsync(int id)
        {
            var menuItem = await _unitOfWork.GetRepository<MenuItem>().GetByIdAsync(id);
            if (menuItem is null) throw new Exception("MenuItem not found");

            menuItem.IsDeleted = true;
            menuItem.DeletedAt = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync();
        }
    }
}

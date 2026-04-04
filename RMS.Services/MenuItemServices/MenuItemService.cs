using AutoMapper;
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
    }
}

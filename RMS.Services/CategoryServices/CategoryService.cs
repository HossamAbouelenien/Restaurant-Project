using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using RMS.Domain.Contracts;
using RMS.Domain.Entities;
using RMS.Services.Specifications.CategorySpec;
using RMS.ServicesAbstraction.ICategoriesService;
using RMS.Shared.DTOs.CategoryDTOs;

namespace RMS.Services.CategoryServices
{
    public class CategoryService : ICategoriesService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryService(IUnitOfWork unitOfWork , IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
        }


        public async Task<IEnumerable<CategoryDTO>> GetAllCategoriesAsync()
        {
            var Spec = new CategoryWithIncludingMenuItemOrderedByName();
            var Categories = await _unitOfWork.GetRepository<Category>().GetAllAsync(Spec);
            var result = _mapper.Map<IEnumerable<CategoryDTO>>(Categories);
            return result;

        }
    }
}

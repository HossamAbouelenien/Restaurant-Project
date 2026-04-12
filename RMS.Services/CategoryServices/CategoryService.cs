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
    public class CategoryService : ICategoryService
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

        public async Task<CategoryDTO?> GetCategoryByIdAsync(int id)
        {

            var Spec = new GetCategoryByIDWithIncludingMenutItems(id);
            var Category = await _unitOfWork.GetRepository<Category>().GetByIdAsync(Spec);
            var result = _mapper.Map<CategoryDTO>(Category);

            return result;

        }


        public async Task<CategoryDTO> AddCategoryAsync(CreateCategoryDTO DTO)
        {

            if(string.IsNullOrEmpty(DTO.Name))
            {
                throw new Exception("Category name is required");
            }

            var repository = _unitOfWork.GetRepository<Category>();

            var ExitingCategories = await repository.GetAllAsync();

            if (ExitingCategories.Any(C => C.Name.ToLower() == DTO.Name.ToLower()))
            {
                throw new Exception("Category already exists");
            }

            var Category = _mapper.Map<Category>(DTO);

            Category.CreatedAt = DateTime.UtcNow;

            await repository.AddAsync(Category);

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<CategoryDTO>(Category);


        }

        public async Task<CategoryDTO> UpdateCategoryAsync(int id, UpdateCategoryDTO DTO)
        {
            var repository = _unitOfWork.GetRepository<Category>();

            var Category = await repository.GetByIdAsync(id);

            if(Category == null || Category.IsDeleted)
            {
                throw new Exception("Category not found");
            }

            _mapper.Map(DTO,Category);

            Category.UpdatedAt = DateTime.UtcNow;

            repository.Update(Category);

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<CategoryDTO>(Category);

        }

        public Task DeleteCategoryAsync(int id)
        {
            



        }
    }
}

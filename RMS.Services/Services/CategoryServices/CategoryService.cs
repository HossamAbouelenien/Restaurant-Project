using AutoMapper;
using RMS.Domain.Contracts;
using RMS.Domain.Entities;
using RMS.Services.Exceptions;
using RMS.Services.Specifications.CategorySpec;
using RMS.ServicesAbstraction.ICategoriesService;
using RMS.Shared.DTOs.CategoryDTOs;

namespace RMS.Services.Services.CategoryServices
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

            if(Category is null)
            {
                throw new CategoryNotFoundException(id);
            }

            var result = _mapper.Map<CategoryDTO>(Category);

            return result;

        }





        public async Task<CategoryDTO> AddCategoryAsync(CreateCategoryDTO DTO)
        {

            if(string.IsNullOrEmpty(DTO.Name))
            {
                throw new CategoryNameRequiredException();
            }

            var repository = _unitOfWork.GetRepository<Category>();

            var ExitingCategories = await repository.GetAllAsync();

            if (ExitingCategories.Any(C => C.Name.ToLower() == DTO.Name.ToLower()))
            {
                throw new CategoryAlreadyExistsException(DTO.Name);
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
                throw new CategoryNotFoundException(id);
            }

            _mapper.Map(DTO,Category);

            Category.UpdatedAt = DateTime.UtcNow;

            repository.Update(Category);

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<CategoryDTO>(Category);

        }

        public async Task DeleteCategoryAsync(int id)
        {
            
            var repository = _unitOfWork.GetRepository<Category>();

            var Spec = new CategoryWithIncludingMenuItemsByIDForDelete(id);

            var Category = await repository.GetByIdAsync(Spec);

            if(Category == null || Category.IsDeleted)
            {
                throw new CategoryNotFoundException(id);
            }

            if (Category.MenuItems.Any())
            {
                throw new CategoryHasMenuItemsException(id);
            }

            Category.IsDeleted = true;
            Category.DeletedAt = DateTime.UtcNow;

            repository.Update(Category);

            await _unitOfWork.SaveChangesAsync();



        }
    }
}

using AutoMapper;
using RMS.Domain.Contracts;
using RMS.Domain.Entities;
using RMS.Services.Exceptions;
using RMS.Services.Specifications.IngredientSpec;
using RMS.ServicesAbstraction.IServices.IIngredientServices;
using RMS.Shared;
using RMS.Shared.DTOs.IngredientDTOs;

namespace RMS.Services.Services.IngredientServices
{
    public class IngredientService : IIngredientService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public IngredientService(IUnitOfWork unitOfWork ,IMapper mapper )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            
        }

        // Get all Ingredients
        public async Task<PaginatedResult<IngredientDTO>> GetAllIngredientsAsync(int pageIndex, int pageSize)
        {
            var repo = _unitOfWork.GetRepository<Ingredient>();

            var spec = new IngredientSpecification(pageIndex, pageSize);
            var countSpec = new IngredientCountSpecification();

            var ingredients = await repo.GetAllAsync(spec);
            var totalCount = await repo.CountAsync(countSpec);

            var data = _mapper.Map<IEnumerable<IngredientDTO>>(ingredients);

            return new PaginatedResult<IngredientDTO>(
                pageIndex,
                pageSize,
                totalCount,
                data
            );
        }



        public async Task<IngredientDTO> GetIngredientByIdAsync(int id)
        {
            var ingerdient = await _unitOfWork.GetRepository<Ingredient>().GetByIdAsync(id);

            if(ingerdient == null)
            {
                throw new IngredientNotFoundException(id);

            }

            var result = _mapper.Map<IngredientDTO>(ingerdient);
            return result;
        }





        public async Task<IngredientDTO> CreateIngredientAsync(CreateIngredientDTO dto)
        {
            var repo =  _unitOfWork.GetRepository<Ingredient>();

            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                throw new IngredientNameRequiredException();
            }
                

            var existing = await repo.GetAllAsync();

            if (existing.Any(i => i.Name.ToLower() == dto.Name.ToLower()))
            {
                throw new IngredientAlreadyExistsException(dto.Name);
            }
                

            var ingredient = _mapper.Map<Ingredient>(dto);

            await repo.AddAsync(ingredient);

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<IngredientDTO>(ingredient);
        }





        public async Task DeleteIngredientAsync(int id)
        {
            var repo = _unitOfWork.GetRepository<Ingredient>();

            var ingredient = await repo.GetByIdAsync(id);

            if (ingredient is null)
            {
                throw new IngredientNotFoundException(id);
            }

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


            if (ingredient is null)
                throw new IngredientNotFoundException(id);

            if (string.IsNullOrWhiteSpace(updateIngredientDTO.Name))
            {
                throw new IngredientNameRequiredException();
            }
                

            var existing = await repo.GetAllAsync();

            if (existing.Any(i => i.Name.ToLower() == updateIngredientDTO.Name.ToLower()))
                throw new IngredientAlreadyExistsException(updateIngredientDTO.Name);

            repo.Update(ingredient!);

            ingredient.UpdatedAt = DateTime.UtcNow;
            await _unitOfWork.SaveChangesAsync();
            
            var res = _mapper.Map<IngredientDTO>(ingredient);

            return res;
             


        }



    }














}






























using AutoMapper;
using RMS.Domain.Contracts;
using RMS.Domain.Entities;
using RMS.ServicesAbstraction;
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



        public async Task<IEnumerable<IngredientDTO>> GetAllIngredientsAsync()
        {
            var ingredients = await _unitOfWork.GetRepository<Ingredient>().GetAllAsync();
            var result = _mapper.Map<IEnumerable<IngredientDTO>>(ingredients);
            return result;
        }
    }
}

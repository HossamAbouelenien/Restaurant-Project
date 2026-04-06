using AutoMapper;
using RMS.Domain.Contracts;
using RMS.Domain.Entities;
using RMS.Services.Specifications.Tablespec;
using RMS.ServicesAbstraction;
using RMS.Shared.DTOs.TableDTOs;
using RMS.Shared.QueryParams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Services.TableServices
{
    public class TableService : ITableService

    {
        
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TableService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<TableDTO> CreateTableAsync(CreateTableDTO dto)
        {
            var repo = _unitOfWork.GetRepository<Table>();
            var branch=  await _unitOfWork.GetRepository<Branch>().GetByIdAsync(dto.BranchId);
            if (branch == null) throw new Exception("Branch not found");
            var specBranchId = new TableBranchIdSpecification(dto.BranchId);
             var tablesFromDb= await repo.GetAllAsync(specBranchId);
            if (tablesFromDb.Any(t => t.TableNumber == dto.TableNumber))
                throw new Exception("Table is already exsit");
            if (dto.Capacity < 1 || dto.Capacity > 20) throw new Exception("Capcity must be between 1 and 20");
            var table = _mapper.Map<Table>(dto);
            await repo.AddAsync(table);
            await _unitOfWork.SaveChangesAsync();
            var spec = new TableSpecification(table.Id);
            var tableWithIncludes = await repo.GetByIdAsync(spec);
            return _mapper.Map<TableDTO>(tableWithIncludes);
        }
        public async Task<IEnumerable<TableDTO>> GetAllTablesAsync(TableQueryParams queryParams)
        {
            var repo = _unitOfWork.GetRepository<Table>();
            var spec =  new TableSpecification(queryParams);
            var tables = await repo.GetAllAsync(spec); 
            return _mapper.Map<IEnumerable<TableDTO>>(tables);
        }
    }
}

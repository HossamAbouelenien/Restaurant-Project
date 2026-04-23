using AutoMapper;
using RMS.Domain.Contracts;
using RMS.Domain.Entities;
using RMS.Services.Specifications.Tablespec;
using RMS.ServicesAbstraction;
using RMS.Shared;
using RMS.Shared.DTOs.TableDTOs;
using RMS.Shared.QueryParams;
using RMS.Shared.SharedResources;
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

            if (branch == null) 
                throw new Exception(SharedResourcesKeys.NotFound);

            var specBranchId = new TableBranchIdSpecification(dto.BranchId);
            var tablesFromDb= await repo.GetAllAsync(specBranchId);

            if (tablesFromDb.Any(t => t.TableNumber == dto.TableNumber))
                throw new Exception(SharedResourcesKeys.AlreadyExists);

            if (dto.Capacity < 1 || dto.Capacity > 20)
                throw new Exception(SharedResourcesKeys.TableCapacityInvalid);

            var table = _mapper.Map<Table>(dto);
            await repo.AddAsync(table);
            await _unitOfWork.SaveChangesAsync();

            var spec = new TableSpecification(table.Id);
            var tableWithIncludes = await repo.GetByIdAsync(spec);
            return _mapper.Map<TableDTO>(tableWithIncludes);
        }

        public async Task DeleteTableAsync(int id)
        {
            var repo = _unitOfWork.GetRepository<Table>();
            var spec = new TableSpecification(id);
            var table = await repo.GetByIdAsync(spec);

            if (table is null)
                throw new Exception(SharedResourcesKeys.NotFound);

            if (table.IsOccupied)
                throw new Exception(SharedResourcesKeys.OccupiedTable);

            table.IsDeleted = true;
            table.DeletedAt = DateTime.UtcNow;
            repo.Update(table);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<PaginatedResult<TableDTO>> GetAllTablesAsync(TableQueryParams queryParams)
        {
            var repo = _unitOfWork.GetRepository<Table>();
            var spec = new TableSpecification(queryParams);
            var tables = await repo.GetAllAsync(spec);
            var tableDtos = _mapper.Map<IEnumerable<TableDTO>>(tables);
            var countSpec = new TableCountSpecification(queryParams);
            var countOfTables = await repo.CountAsync(countSpec);
            var pageSize = tableDtos.Count();
            var paginatedResult = new PaginatedResult<TableDTO>(
                queryParams.PageIndex,
                pageSize,
                countOfTables,
                tableDtos
            );
            return paginatedResult;
        }
        public async Task<TableDTO> GetTableByIdAsync(int id)
        {
            var repo = _unitOfWork.GetRepository<Table>();
            var spec = new TableWithTableOrderAndOrderAndBranchSpecification(id);
            var table = await repo.GetByIdAsync(spec);

            if (table == null)
                throw new Exception(SharedResourcesKeys.NotFound);

            return _mapper.Map<TableDTO>(table);
        }

        public async Task<TableDTO> UpdateTableAsync(int id, UpdateTableDTO dto)
        {
            var repo = _unitOfWork.GetRepository<Table>();
            var spec = new TableSpecification(id);
            var tableFromDb = await repo.GetByIdAsync(spec);

            if (tableFromDb == null)
                throw new Exception(SharedResourcesKeys.NotFound);

            dto.TableNumber = dto.TableNumber?.Trim();

            if (string.IsNullOrWhiteSpace(dto.TableNumber))
                throw new Exception(SharedResourcesKeys.Required);

            if (dto.Capacity < 1 || dto.Capacity > 20)
                throw new Exception(SharedResourcesKeys.TableCapacityInvalid);

            var branchSpec = new TableBranchIdSpecification(tableFromDb.BranchId);
            var tablesInSameBranch = await repo.GetAllAsync(branchSpec);

            if (tablesInSameBranch.Any(t => t.Id != id &&
                t.TableNumber.Trim().ToLower() == dto.TableNumber.ToLower()))
            {
                throw new Exception(SharedResourcesKeys.TableBranch);
            }

            _mapper.Map(dto, tableFromDb);

            repo.Update(tableFromDb);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<TableDTO>(tableFromDb);
        }
        
        public async Task<IEnumerable<TableOrderDTO>> GetAllTableOrdersAsync(TableOrderQueryParams queryParams)
        {
            var repo = _unitOfWork.GetRepository<TableOrder>();
            var spec = new TableOrderSpecification(queryParams);
            var tableOrders = await repo.GetAllAsync(spec);

            if (queryParams.TableId.HasValue)
            {
                var tableRepo = _unitOfWork.GetRepository<Table>();
                var tableSpec = new TableSpecification(queryParams.TableId.Value);
                var table = await tableRepo.GetByIdAsync(tableSpec);

                if (table is null)
                    throw new Exception(SharedResourcesKeys.NotFound);
            }

            return _mapper.Map<IEnumerable<TableOrderDTO>>(tableOrders);
        }
        public async Task ToggleTableStatusAsync(int id)
        {
            var repo = _unitOfWork.GetRepository<Table>();
            var spec = new TableWithOrdersSpecification(id);
            var table = await repo.GetByIdAsync(spec);

            if (table is null)
                throw new Exception(SharedResourcesKeys.NotFound);

            if (table.IsOccupied == true && table.TableOrders.Any(to => to.CompletedAt == null))
                throw new Exception(SharedResourcesKeys.FreeTable);

            table.IsOccupied = !table.IsOccupied;
            table.UpdatedAt = DateTime.UtcNow;
            repo.Update(table);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task<TableOrderDTO> CompleteTableOrderAsync(int id)
        {
            var repo = _unitOfWork.GetRepository<TableOrder>();

            var tableOrder = await repo.GetByIdAsync(id);

            if (tableOrder is null)
                throw new Exception(SharedResourcesKeys.NotFound);

            if (tableOrder.CompletedAt != null)
                throw new Exception(SharedResourcesKeys.CompletedTableOrder);

            tableOrder.CompletedAt = DateTime.UtcNow;

            repo.Update(tableOrder);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<TableOrderDTO>(tableOrder);
        }

    }
}

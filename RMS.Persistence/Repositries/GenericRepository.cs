using Microsoft.EntityFrameworkCore;
using RMS.Domain.Contracts;
using RMS.Domain.Entities;
using RMS.Persistence.Data.Contexts;

namespace RMS.Persistence.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly AppDbContext _dbContex;
        public GenericRepository(AppDbContext dbContex)
        {
            _dbContex = dbContex;
        }
        public async Task AddAsync(TEntity entity) => await _dbContex.Set<TEntity>().AddAsync(entity);    
        public async Task<IEnumerable<TEntity>> GetAllAsync() => await _dbContex.Set<TEntity>().ToListAsync();
        public async Task<TEntity?> GetByIdAsync(int id) => await _dbContex.Set<TEntity>().FindAsync(id);
        public void Remove(TEntity entity) => _dbContex.Set<TEntity>().Remove(entity);
        public void Update(TEntity entity) => _dbContex.Set<TEntity>().Update(entity);

        public async Task<IEnumerable<TEntity>> GetAllAsync(ISpecifications<TEntity> specifications)
        {
            var Query = SpecificationsEvaluator.CreateQuery<TEntity>(_dbContex.Set<TEntity>(), specifications);

            return await Query.ToListAsync();
        }
        
        public async Task<TEntity?> GetByIdAsync(ISpecifications<TEntity> specifications)
        {
            return await SpecificationsEvaluator.CreateQuery(_dbContex.Set<TEntity>(), specifications).FirstOrDefaultAsync();
        }
    }
    }

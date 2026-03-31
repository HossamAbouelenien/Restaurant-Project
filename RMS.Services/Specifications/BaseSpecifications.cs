using RMS.Domain.Contracts;
using RMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Services.Specifications
{
    internal abstract class BaseSpecifications<TEntity> : ISpecifications<TEntity> where TEntity : BaseEntity
    {
        #region Sorting
        public Expression<Func<TEntity, object>> OrderBy { get; private set; } =default!;

        public Expression<Func<TEntity, object>> OrderByDescending { get; private set; } = default!;

        protected void AddOrderBy(Expression<Func<TEntity, object>> orderByExpression)
        {
            OrderBy = orderByExpression;
        }
        protected void AddOrderByDescending(Expression<Func<TEntity, object>> orderByDescendingExpression)
        {
            OrderByDescending = orderByDescendingExpression;
        }


        #endregion

        #region Criteria
        public Expression<Func<TEntity, bool>> Criteria { get; } 
        protected BaseSpecifications(Expression<Func<TEntity, bool>> criteriaExpression)
        {
            Criteria = criteriaExpression;
        }
        #endregion

        #region Includes
        public ICollection<Expression<Func<TEntity, object>>> IncludeExpressions { get; } = [];
        protected void AddInclude(Expression<Func<TEntity, object>> includeExp)
        {
            IncludeExpressions.Add(includeExp);
        }
        #endregion

        #region Pagination

        public int Take { get; private set; }
        public int Skip { get; private set; }
        public bool IsPaginated { get; private set; }
        protected void ApplyPagination(int PageSize, int pageIndex)
        {
            IsPaginated = true;
            Take = PageSize;
            Skip = (pageIndex - 1) * PageSize;
        }

        #endregion

        
    }
}

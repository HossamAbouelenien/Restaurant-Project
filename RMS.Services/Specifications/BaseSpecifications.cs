using RMS.Domain.Contracts;
using System.Linq.Expressions;

namespace RMS.Services.Specifications
{
    public abstract class BaseSpecifications<TEntity> : ISpecifications<TEntity> where TEntity : class
    {
        #region Sorting

        public Expression<Func<TEntity, object>> OrderBy { get; private set; } = default!;

        public Expression<Func<TEntity, object>> OrderByDescending { get; private set; } = default!;

        protected void AddOrderBy(Expression<Func<TEntity, object>> orderByExpression)
        {
            OrderBy = orderByExpression;
        }

        protected void AddOrderByDescending(Expression<Func<TEntity, object>> orderByDescendingExpression)
        {
            OrderByDescending = orderByDescendingExpression;
        }

        #endregion Sorting

        #region Criteria

        public Expression<Func<TEntity, bool>> Criteria { get; }

        protected BaseSpecifications(Expression<Func<TEntity, bool>> criteriaExpression)
        {
            Criteria = criteriaExpression;
        }

        #endregion Criteria

        #region Includes

        public ICollection<Expression<Func<TEntity, object>>> IncludeExpressions { get; } = [];

        protected void AddInclude(Expression<Func<TEntity, object>> includeExp)
        {
            IncludeExpressions.Add(includeExp);
        }
        public ICollection<string> IncludeStrings { get; } = [];
        protected void AddInclude(string includeString)
        {
            IncludeStrings.Add(includeString);
        }

        #endregion Includes

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

        #endregion Pagination

        #region Ignore Query Filter
        public bool IgnoreQueryFilters { get; protected set; } = false;
        #endregion
    }
}
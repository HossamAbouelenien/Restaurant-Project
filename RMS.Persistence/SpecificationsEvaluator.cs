using Microsoft.EntityFrameworkCore;
using RMS.Domain.Contracts;

namespace RMS.Persistence
{
    public static class SpecificationsEvaluator
    {
        public static IQueryable<TEntity> CreateQuery<TEntity>(IQueryable<TEntity> entryPoint,
                                                      ISpecifications<TEntity> specifications) where TEntity : class
        {
            var Query = entryPoint; // _dbContext.Products
            if (specifications is not null)
            {
                if (specifications.IgnoreQueryFilters)
                {
                    Query = Query.IgnoreQueryFilters();
                }

                if (specifications.Criteria is not null)
                {
                    Query = Query.Where(specifications.Criteria); 
                }

                if (specifications.IncludeExpressions is not null && specifications.IncludeExpressions.Any())
                {
                    Query = specifications.IncludeExpressions.Aggregate(Query,
                        (CurrentQuery, IncludeExp) => CurrentQuery.Include(IncludeExp));
                }

                if (specifications.IncludeStrings is not null && specifications.IncludeStrings.Any())
                {
                    Query = specifications.IncludeStrings
                       .Aggregate(Query, (CurrentQuery, includeString) => CurrentQuery.Include(includeString));
                }

                if (specifications.OrderBy is not null)
                {
                    Query = Query.OrderBy(specifications.OrderBy);
                }

                if (specifications.OrderByDescending is not null)
                {
                    Query = Query.OrderByDescending(specifications.OrderByDescending);
                }

                if (specifications.IsPaginated)
                {
                    Query = Query.Skip(specifications.Skip).Take(specifications.Take);
                }
               
            }
            return Query;
        }
    }
}
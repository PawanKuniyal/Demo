using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Helpa.Services.Repository
{
    public static class IncludeMultiple
    {
        public static IQueryable<T> IncludeEntities<T>(this IQueryable<T> query, params Expression<Func<T, object>>[] includes)
            where T: class
        {
            if (includes != null)
            {
                query = includes.Aggregate(query,
                        (current, include) => current.Include(include));
            }
            return query;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Helpa.Services.Repository
{
    public interface IRepository<T>
        where T : class
    {
        IQueryable<T> GetAll(Expression<Func<T, bool>> predicate = null);
        IQueryable<T> GetById(Expression<Func<T, bool>> predicate = null);
        T GetEntity(Expression<Func<T, bool>> predicate = null);
        Task<T> GetEntityAsync(Expression<Func<T, bool>> predicate = null);
        T Insert(T entity);
        Task<T> InsertAsync(T Entity);
        List<T> Insert(List<T> entities);
        int Update(T entity);
        int UpdatePartial(T entity, params Expression<Func<T, object>>[] updatedProperties);
        int Delete(T entity);
        int Delete(List<T> entities);
        Task<T> FindByIdAsync(int Id);
        Task<int> UpdateAsync(T entity);
    }
}

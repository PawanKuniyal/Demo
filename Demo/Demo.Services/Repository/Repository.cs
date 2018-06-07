using System;
using System.Collections.Generic;
using System.Linq;
using Helpa.Entities.Context;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Helpa.Services.Repository
{
    public class Repository<T> : IRepository<T>, IDisposable
        where T : class, new()
    {
        private HelpaContext context;

        /// <summary>
        /// Public Constructor,initializes privately declared local variables.
        /// </summary>
        /// <param name="context"></param>
        public Repository()
        {
            context = new HelpaContext();
        }

        /// <summary>
        /// Get all related rows
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IQueryable<T> GetAll(Expression<Func<T, bool>> predicate = null)
        {
            var data = context.Set<T>();

            if (predicate != null)
            {
                var dbObj = data.Where(predicate);
                return dbObj;
            }

            return data;
        }

        /// <summary>
        /// It will returns only top first record, use get all for list of records
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IQueryable<T> GetById(Expression<Func<T, bool>> predicate = null)
        {
            var data = context.Set<T>();
            var dbObjects = data.Where(predicate);
            return dbObjects;
        }

        /// <summary>
        /// It will returns only top first record, use get all for list of records
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public T GetEntity(Expression<Func<T, bool>> predicate = null)
        {
            var data = context.Set<T>();
            var dbObjects = data.SingleOrDefault(predicate);
            return dbObjects;
        }

        public async Task<T> GetEntityAsync(Expression<Func<T, bool>> predicate = null)
        {
            var data = context.Set<T>();
            var dbObject = await data.SingleOrDefaultAsync(predicate);
            return dbObject;
        }

        /// <summary>
        /// To Insert entity in database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public T Insert(T entity)
        {
            context.Set<T>().Add(entity);
            context.SaveChanges();

            return entity;
        }

        /// <summary>
        /// Insert entity async
        /// </summary>
        /// <param name="Entity"></param>
        /// <returns></returns>
        public async Task<T> InsertAsync(T Entity)
        {
            context.Set<T>().Add(Entity);
            await context.SaveChangesAsync();

            return Entity;
        }

        /// <summary>
        /// To Insert entities in database
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public List<T> Insert(List<T> entities)
        {
            context.Set<T>().AddRange(entities);
            context.SaveChanges();

            return entities;
        }
        
        /// <summary>
        /// To update entity in database
        /// </summary>
        /// <param name="entity"></param>
        public int Update(T entity)
        {
            using (HelpaContext localContext = new HelpaContext())
            {
                localContext.Set<T>().Attach(entity);
                localContext.Entry(entity).State = EntityState.Modified;
                int result = localContext.SaveChanges();
                return result;
            }
        }

        /// <summary>
        /// To update entity in database async
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(T entity)
        {
            using (HelpaContext lContext = new HelpaContext())
            {
                lContext.Set<T>().Attach(entity);
                lContext.Entry(entity).State = EntityState.Modified;
                int result = await lContext.SaveChangesAsync();
                return result;
            }
        }

        /// <summary>
        /// To update entity properties in database
        /// </summary>
        /// <param name="entity"></param>
        public int UpdatePartial(T entity, params Expression<Func<T, object>>[] updatedProperties)
        {
            using (HelpaContext localContext = new HelpaContext())
            {
                localContext.Set<T>().Attach(entity);
                if (updatedProperties.Any())
                {
                    //update explicitly mentioned properties
                    foreach (var property in updatedProperties)
                    {
                        localContext.Entry(entity).Property(property).IsModified = true;
                    }
                }
                else
                {
                    //no items mentioned, so find out the updated entries
                    foreach (var property in localContext.Entry(entity).OriginalValues.PropertyNames)
                    {
                        var original = localContext.Entry(entity).OriginalValues.GetValue<object>(property);
                        var current = localContext.Entry(entity).CurrentValues.GetValue<object>(property);
                        if (original != null && !original.Equals(current))
                            localContext.Entry(entity).Property(property).IsModified = true;
                    }
                }
                int result = localContext.SaveChanges();
                return result;
            }
        }

        /// <summary>
        /// Delete entity from database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Delete(T entity)
        {
            context.Entry(entity).State = System.Data.Entity.EntityState.Deleted;
            int result = context.SaveChanges();
            return result;
        }

        /// <summary>
        /// To Remove entities from database
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public int Delete(List<T> entities)
        {
            entities.ForEach(x => { context.Entry(x).State = System.Data.Entity.EntityState.Deleted; });
            context.Set<T>().RemoveRange(entities);
            int result = context.SaveChanges();

            return result;
        }

        /// <summary>
        /// Find entity by id 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<T> FindByIdAsync(int Id)
        {
            var result = await context.Set<T>().FindAsync(Id);
            return result;
        }

        private bool disposed = false;

        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (context != null)
                    {
                        context.Dispose();
                        context = null;
                    }
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}

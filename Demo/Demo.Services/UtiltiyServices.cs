using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Helpa.Entities;
using Helpa.Services.Interface;
using Helpa.Services.Repository;

namespace Helpa.Services
{
    public class UtiltiyServices : IUtilityServices
    {
        private IRepository<Location1> context;
        private IRepository<Service> serviceContext;
        private IRepository<Scope> scopeContext;

        public UtiltiyServices()
        {
            context = new Repository<Location1>();
            serviceContext = new Repository<Service>();
            scopeContext = new Repository<Scope>();
        }

        #region Location
        public Location1 AddLocation(Location1 location)
        {
            var result = context.Insert(location);
            return result;
        }

        public async Task<Location1> AddLocationAsync(Location1 location)
        {
            var result = await context.InsertAsync(location);
            return result;
        }
        #endregion

        #region Get Services
        public IQueryable<Service> GetServices()
        {
            var result = serviceContext.GetAll(x => x.RowStatus != "D");
            return result;
        }
        #endregion

        #region Get Scopes
        public IQueryable<Scope> GetScopes()
        {
            var result = scopeContext.GetAll(x => x.RowStatus != "D");
            return result;
        }

        public IQueryable<Scope> GetScopeByServiceId(int Id)
        {
            var result = scopeContext.GetById(x => x.RowStatus != "D" && x.ServiceId == Id);
            return result;
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Helpa.Entities;

namespace Helpa.Services.Interface
{
    public interface IUtilityServices
    {
        IQueryable<Service> GetServices();
        IQueryable<Scope> GetScopes();
        IQueryable<Scope> GetScopeByServiceId(int Id);
        Location1 AddLocation(Location1 location);
        Task<Location1> AddLocationAsync(Location1 location);
    }
}

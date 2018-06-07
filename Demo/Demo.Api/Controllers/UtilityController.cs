using System.Linq;
using System.Web.Http;
using Helpa.Entities.CustomEntities;
using Helpa.Services;
using Helpa.Services.Interface;

namespace Helpa.Api.Controllers
{
    public class UtilityController : ApiController
    {
        private IUtilityServices services;

        public UtilityController()
        {
            services = new UtiltiyServices();
        }

        // GET : get all services api/Services
        [Route("api/Services")]
        public IQueryable<ServicesDTO> GetServices()
        {
            var result = services.GetServices().Select(x => new ServicesDTO()
            {
                Id = x.ServiceId,
                ServiceName = x.ServiceName
            });
            return result;
        }

        // GET : get all scopes api/Scopes
        [Route("api/Scopes")]
        public IQueryable<ScopeDTO> GetScope()
        {
            var result = services.GetScopes().Select(x => new ScopeDTO()
            {
                Id = x.ScopeId,
                ScopeName = x.ScopeName
            });
            return result;
        }

        // GET : get all scopes api/Scopes/5
        [Route("api/Scopes/{Id}")]
        public IQueryable<ScopeDTO> GetScopeByServiceId(int Id)
        {
            var result = services.GetScopeByServiceId(Id).Select(x => new ScopeDTO()
            {
                Id = x.ScopeId,
                ScopeName = x.ScopeName
            });
            return result;
        }
    }
}

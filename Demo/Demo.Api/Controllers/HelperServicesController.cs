using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Data.Entity.Spatial;
using Helpa.Entities;
using Helpa.Entities.CustomEntities;
using Helpa.Entities.Context;
using Helpa.Services.Interface;
using Helpa.Services;
using Helpa.Api.Utilities;

namespace Helpa.Api.Controllers
{
    [Authorize(Roles = "HELPER")]
    public class HelperServicesController : ApiController
    {
        private HelpaContext db = new HelpaContext();
        private IHelperServices services;
        private IUtilityServices utilityServices;

        public HelperServicesController()
        {
            services = new Services.HelperServices();
            utilityServices = new UtiltiyServices();
        }

        // GET: api/HelperServices
        [AllowAnonymous]
        public IQueryable<HelperService> GetHelperServices()
        {
            var result = services.GetAllHelpers();
            return result;
        }

        // GET: api/HelperServices/5
        [AllowAnonymous]
        [ResponseType(typeof(HelperService))]
        public async Task<IHttpActionResult> GetHelperService(int id)
        {
            HelperService helperService = await services.GetHelperAsync(id);
            if (helperService == null)
            {
                return NotFound();
            }

            return Ok(helperService);
        }

        // PUT: api/HelperServices/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutHelperService(int id, [FromBody]HelperService helperService)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != helperService.HelperServiceId)
            {
                return BadRequest();
            }

            db.Entry(helperService).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HelperServiceExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/HelperServices
        [ResponseType(typeof(Helper))]
        [AllowAnonymous]
        public async Task<IHttpActionResult> PostHelperService([FromBody]HelperServiceDTO helperServiceDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Helper helper = new Helper
            {
                UserId = helperServiceDTO.UserId,
                Experience = helperServiceDTO.ExperienceYears,
                MaxAgeGroup = helperServiceDTO.MaxAge,
                MinAgeGroup = helperServiceDTO.MinAge,
                Qualification = helperServiceDTO.Qualification,
                RowStatus = "I",
                Status = true,
                CreatedDate = DateTime.UtcNow
            };

            try
            {
                var result = await services.AddHelperAsync(helper);
                var helperId = result.HelperId;
                
                HelperService helperService = null;
                foreach (var item in helperServiceDTO.Service)
                {
                    int LocationId = 0;
                    if (!String.IsNullOrEmpty(item.Latitude) && !String.IsNullOrEmpty(item.Longitude))
                    {
                        DbGeography geography = LocationPoint.CreatePoint(Convert.ToInt64(item.Latitude), Convert.ToInt64(item.Longitude));
                        Location1 location = new Location1
                        {
                            LocationName = item.LocationName,
                            LocationGeography = geography,
                            RowStatus = "I",
                            CreatedDate = DateTime.UtcNow
                        };

                        var locationResult = await utilityServices.AddLocationAsync(location);
                        LocationId = locationResult.LocationId;
                    }

                    helperService = new HelperService
                    {
                        HelperId = helperId,
                        HourPrice = item.Hour,
                        MaxDayPrice = item.MaxDayPrice,
                        MaxHourPrice = item.MaxPriceHour,
                        MaxMonthPrice = item.MaxMonthPrice,
                        MinDayPrice = item.MinDayPrice,
                        MinHourPrice = item.MinPriceHour,
                        MinMonthPrice = item.MinMonthPrice,
                        MonthlyPrice = item.Month,
                        RowStatus = "I",
                        ServiceId = item.ServiceId,
                        Status = true,
                        CreatedDate = DateTime.UtcNow
                    };

                    var serviceResult = await services.AddHelperServiceAsync(helperService);
                    int ServiceId = serviceResult.HelperServiceId;

                    if (item.Scopes.Count != 0)
                    {
                        HelperServiceScope serviceScope = null;
                        foreach (var scope in item.Scopes)
                        {
                            serviceScope = new HelperServiceScope
                            {
                                HelperServiceId = ServiceId,
                                HelperScopeId = scope.ScopeId,
                                RowStatus = "I",
                                CreatedDate = DateTime.UtcNow
                            };
                            var scopResult = await services.AddHelperServiceScopeAsync(serviceScope);
                        }
                    }
                }
            }
            catch (DbUpdateException)
            {
                if (HelperServiceExists(helper.HelperId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }

        // DELETE: api/HelperServices/5
        [ResponseType(typeof(HelperService))]
        public async Task<IHttpActionResult> DeleteHelperService(int id)
        {
            HelperService helperService = await db.HelperServices.FindAsync(id);
            if (helperService == null)
            {
                return NotFound();
            }

            db.HelperServices.Remove(helperService);
            await db.SaveChangesAsync();

            return Ok(helperService);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool HelperServiceExists(int id)
        {
            return db.HelperServices.Count(e => e.HelperServiceId == id) > 0;
        }
    }
}
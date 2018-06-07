using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Helpa.Entities;
using Helpa.Entities.Context;
using Helpa.Entities.CustomEntities;
using Helpa.Services.Interface;

namespace Helpa.Api.Controllers
{
    [Authorize]
    public class HelpersController : ApiController
    {
        private HelpaContext db = new HelpaContext();
        private IHelperServices services;

        public HelpersController()
        {
            services = new Services.HelperServices();
        }

        // GET: api/HelpersList
        [Route("api/Home")]
        [AllowAnonymous]
        public IQueryable<HomeHelpers> GetHelpersList(int radius = 0, double latitude = 0, double longitude = 0)
        {
            var result = services.GetHelpers(radius, latitude, longitude);
            return result;
        }

        // GET: api/HelpersHome
        [Route("api/HelpersHome")]
        [AllowAnonymous]
        public IQueryable<object> GetHelperHome([FromUri]SearchParams searchParams)
        {
            var result = services.GetClusteredHelpers(searchParams);
            return result.AsQueryable();
        }

        [ResponseType(typeof(HomeHelpers))]
        [Route("api/GetHelperDetail")]
        public IHttpActionResult GetHelperDetail(string Email = "", string PhoneNumber = "")
        {
            dynamic result;
            if (!String.IsNullOrEmpty(Email))
            {
                result = services.GetHelperByEmail(Email);
                return Ok(result);
            }
            else if (!String.IsNullOrEmpty(PhoneNumber))
            {
                result = services.GetHelperByPhone(PhoneNumber);
                return Ok(result);
            }
            else
            {
                return NotFound();
            }
        }

        // GET: api/Helpers/5
        [ResponseType(typeof(Helper))]
        public async Task<IHttpActionResult> GetHelper(int id)
        {
            Helper helper = await db.Helpers.FindAsync(id);
            if (helper == null)
            {
                return NotFound();
            }

            return Ok(helper);
        }

        // PUT: api/Helpers/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutHelper(int id, Helper helper)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != helper.HelperId)
            {
                return BadRequest();
            }

            db.Entry(helper).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HelperExists(id))
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

        // POST: api/Helpers
        [ResponseType(typeof(Helper))]
        public async Task<IHttpActionResult> PostHelper(Helper helper)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Helpers.Add(helper);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = helper.HelperId }, helper);
        }

        // DELETE: api/Helpers/5
        [ResponseType(typeof(Helper))]
        public async Task<IHttpActionResult> DeleteHelper(int id)
        {
            Helper helper = await db.Helpers.FindAsync(id);
            if (helper == null)
            {
                return NotFound();
            }

            db.Helpers.Remove(helper);
            await db.SaveChangesAsync();

            return Ok(helper);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool HelperExists(int id)
        {
            return db.Helpers.Count(e => e.HelperId == id) > 0;
        }
    }
}
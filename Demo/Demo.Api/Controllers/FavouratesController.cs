using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Helpa.Entities;
using Helpa.Entities.CustomEntities;
using Helpa.Services;
using Helpa.Services.Interface;

namespace Helpa.Api.Controllers
{
    public class FavouratesController : ApiController
    {
        private IFavourateService favourateService = null;

        public FavouratesController()
        {
            favourateService = new FavourateService();
        }

        // GET: api/Favourates
        public IQueryable<Favourate> GetFavourates()
        {
            var result = favourateService.GetFavourates();
            return result;
        }

        // GET: api/Favourates/5
        public IQueryable<Favourate> GetFavouratesByUserId(int Id)
        {
            var result = favourateService.GetFavourate(Id);
            return result;
        }

        // GET: api/Favourates/5
        [ResponseType(typeof(Favourate))]
        public async Task<IHttpActionResult> GetFavourate(int id)
        {
            Favourate favourate = await favourateService.GetFavourateAsync(id);
            if (favourate == null)
            {
                return NotFound();
            }

            return Ok(favourate);
        }

        // PUT: api/Favourates/5
        //[ResponseType(typeof(void))]
        //public async Task<IHttpActionResult> PutFavourate(int id, [FromBody]FavouratesDTO favouratesDTO)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (!FavourateExists(id))
        //    {
        //        return BadRequest();
        //    }

        //    var fav = favourateService.GetFavourateById(id);

        //    Favourate favourate = new Favourate();

        //    db.Entry(favourate).State = EntityState.Modified;

        //    try
        //    {
        //        await db.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!FavourateExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return StatusCode(HttpStatusCode.NoContent);
        //}

        // POST: api/Favourates
        [ResponseType(typeof(Favourate))]
        public async Task<IHttpActionResult> PostFavourate([FromBody]FavouratesDTO favouratesDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Favourate favourate = new Favourate
            {
                HelperId = favouratesDTO.HelperId,
                UserId = favouratesDTO.UserId
            };

            await favourateService.AddFavourateAsync(favourate);

            return Ok(favourate.FavourateId);
        }

        // DELETE: api/Favourates/5
        [ResponseType(typeof(Favourate))]
        public async Task<IHttpActionResult> DeleteFavourate(int id)
        {
            Favourate favourate = await favourateService.FindFavourateAsync(id);
            if (favourate == null)
            {
                return NotFound();
            }

            await favourateService.RemoveFavourateAsync(favourate);

            return Ok(favourate);
        }

        private bool FavourateExists(int id)
        {
            var result = favourateService.FindFavourateAsync(id);
            if (result != null)
                return true;
            return false;
        }
    }
}
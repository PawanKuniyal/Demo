using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Helpa.Entities;
using Helpa.Entities.Context;
using Helpa.Services;
using Helpa.Services.Interface;
using Helpa.Entities.CustomEntities;

namespace Helpa.Api.Controllers
{
    public class ReviewAndRatingsController : ApiController
    {
        private HelpaContext db = new HelpaContext();
        private IReviewServices reviewServices;

        public ReviewAndRatingsController()
        {
            reviewServices = new ReviewServices();
        }

        // GET: api/ReviewAndRatings
        public IQueryable<ReviewAndRating> GetReviewAndRatings()
        {
            var result = reviewServices.GetAllReviews();
            return result;
        }
        
        public IQueryable<ReviewAndRating> GetReviewAndRatingByHelper(int id)
        {
            var reviewAndRating = reviewServices.GetReviewByHelperId(id);
            if (reviewAndRating == null)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }

            return reviewAndRating;
        }

        // GET: api/ReviewAndRatings/5
        [ResponseType(typeof(ReviewAndRating))]
        public async Task<IHttpActionResult> GetReviewAndRating(int id)
        {
            ReviewAndRating reviewAndRating = await reviewServices.GetReviewAndRatingAsync(id);
            if (reviewAndRating == null)
            {
                return NotFound();
            }

            return Ok(reviewAndRating);
        }

        // PUT: api/ReviewAndRatings/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutReviewAndRating(int id, [FromBody]ReviewAndRatingDTO reviewAndRatingDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (await reviewServices.FindReviewAndRatingAsync(id) == null)
            {
                return BadRequest();
            }

            var review = await reviewServices.GetReviewAndRatingAsync(id);
            ReviewAndRating reviewAndRating = new ReviewAndRating
            {
                CreatedDate = review.CreatedDate,
                HelperId = review.HelperId,
                ReviewId = review.ReviewId,
                UserId = review.UserId
            };
            if (reviewAndRatingDTO.Ratings != null)
            {
                reviewAndRating.Rating = (int)reviewAndRatingDTO.Ratings;
            }
            else
            {
                reviewAndRating.Rating = review.Rating;
            }
            if (reviewAndRatingDTO != null)
            {
                reviewAndRating.Review = reviewAndRatingDTO.Reviews;
            }
            else
            {
                reviewAndRating.Review = review.Review;
            }

            try
            {
                await reviewServices.UpdateReviewAndRatingAsync(reviewAndRating);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReviewAndRatingExists(id))
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

        // POST: api/ReviewAndRatings
        [ResponseType(typeof(ReviewAndRating))]
        public async Task<IHttpActionResult> PostReviewAndRating([FromBody]ReviewAndRatingDTO reviewAndRatingDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ReviewAndRating reviewAndRating = new ReviewAndRating
            {
                HelperId = reviewAndRatingDTO.HelperId,
                UserId = reviewAndRatingDTO.UserId,
                Rating = (int)reviewAndRatingDTO.Ratings,
                Review = reviewAndRatingDTO.Reviews
            };

            await reviewServices.AddReviewAndRatingAsync(reviewAndRating);

            return Ok(reviewAndRating.ReviewId);
        }

        // PUT: api/ReviewAndRatings/5
        [ResponseType(typeof(void))]
        [HttpPut]
        [Route("api/RemoveReview")]
        public async Task<IHttpActionResult> RemoveReviewAndRating(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (await reviewServices.FindReviewAndRatingAsync(id) == null)
            {
                return BadRequest();
            }

            var review = await reviewServices.GetReviewAndRatingAsync(id);

            try
            {
                await reviewServices.RemoveReviewAndRatingAsync(review);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReviewAndRatingExists(id))
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

        // DELETE: api/ReviewAndRatings/5
        [ResponseType(typeof(ReviewAndRating))]
        public async Task<IHttpActionResult> DeleteReviewAndRating(int id)
        {
            ReviewAndRating reviewAndRating = await db.ReviewAndRatings.FindAsync(id);
            if (reviewAndRating == null)
            {
                return NotFound();
            }

            db.ReviewAndRatings.Remove(reviewAndRating);
            await db.SaveChangesAsync();

            return Ok(reviewAndRating);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ReviewAndRatingExists(int id)
        {
            var result = reviewServices.GetReviewAndRating(id);
            if (result != null)
                return true;
            return false;
        }
    }
}
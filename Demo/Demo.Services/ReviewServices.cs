using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Helpa.Entities;
using Helpa.Entities.Context;
using Helpa.Services.Interface;
using Helpa.Services.Repository;

namespace Helpa.Services
{
    public class ReviewServices : IReviewServices
    {
        private HelpaContext context;
        private IRepository<ReviewAndRating> reviewContext;

        public ReviewServices()
        {
            context = new HelpaContext();
            reviewContext = new Repository<ReviewAndRating>();
        }

        #region Review and Rating
        public IQueryable<ReviewAndRating> GetAllReviews()
        {
            var result = reviewContext.GetAll(x => x.RowStatus != "D").IncludeEntities(x => x.ReviewComments);
            return result;
        }

        public IQueryable<ReviewAndRating> GetReviewByHelperId(int Id)
        {
            var result = reviewContext.GetById(x => x.RowStatus != "D" && x.HelperId == Id).IncludeEntities(x => x.ReviewComments);
            return result;
        }

        public ReviewAndRating GetReviewAndRating(int Id)
        {
            var result = reviewContext.GetEntity(x => x.RowStatus != "D" && x.ReviewId == Id);
            return result;
        }

        public async Task<ReviewAndRating> GetReviewAndRatingAsync(int Id)
        {
            var result = await reviewContext.GetEntityAsync(x => x.ReviewId == Id && x.RowStatus != "D");
            return result;
        }

        public ReviewAndRating AddReviewAndRating(ReviewAndRating reviewAndRating)
        {
            reviewAndRating.CreatedDate = DateTime.UtcNow;
            reviewAndRating.RowStatus = "I";
            var result = reviewContext.Insert(reviewAndRating);
            return result;
        }

        public async Task<ReviewAndRating> AddReviewAndRatingAsync(ReviewAndRating reviewAndRating)
        {
            reviewAndRating.CreatedDate = DateTime.UtcNow;
            reviewAndRating.RowStatus = "I";
            var result = await reviewContext.InsertAsync(reviewAndRating);
            return result;
        }

        public int UpdateReviewAndRating(ReviewAndRating reviewAndRating)
        {
            reviewAndRating.UpdatedDate = DateTime.UtcNow;
            reviewAndRating.RowStatus = "U";
            var result = reviewContext.Update(reviewAndRating);
            return result;
        }

        public async Task<int> UpdateReviewAndRatingAsync(ReviewAndRating reviewAndRating)
        {
            reviewAndRating.UpdatedDate = DateTime.UtcNow;
            reviewAndRating.RowStatus = "U";
            var result = await reviewContext.UpdateAsync(reviewAndRating);
            return result;
        }

        public int RemoveReviewAndRating(ReviewAndRating reviewAndRating)
        {
            reviewAndRating.UpdatedDate = DateTime.UtcNow;
            reviewAndRating.RowStatus = "D";
            var result = reviewContext.Update(reviewAndRating);
            return result;
        }

        public async Task<int> RemoveReviewAndRatingAsync(ReviewAndRating reviewAndRating)
        {
            reviewAndRating.UpdatedDate = DateTime.UtcNow;
            reviewAndRating.RowStatus = "D";
            var result = await reviewContext.UpdateAsync(reviewAndRating);
            return result;
        }

        public ReviewAndRating FindReviewAndRating(int Id)
        {
            var result = context.ReviewAndRatings.Find(Id);
            return result;
        }

        public async Task<ReviewAndRating> FindReviewAndRatingAsync(int Id)
        {
            var result = await context.ReviewAndRatings.FindAsync(Id);
            return result;
        }

        #endregion
    }
}

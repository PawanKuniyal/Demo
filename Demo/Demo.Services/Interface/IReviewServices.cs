using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Helpa.Entities;

namespace Helpa.Services.Interface
{
    public interface IReviewServices
    {
        IQueryable<ReviewAndRating> GetAllReviews();
        IQueryable<ReviewAndRating> GetReviewByHelperId(int Id);
        ReviewAndRating GetReviewAndRating(int Id);
        Task<ReviewAndRating> GetReviewAndRatingAsync(int Id);
        ReviewAndRating AddReviewAndRating(ReviewAndRating reviewAndRating);
        Task<ReviewAndRating> AddReviewAndRatingAsync(ReviewAndRating reviewAndRating);
        int UpdateReviewAndRating(ReviewAndRating reviewAndRating);
        Task<int> UpdateReviewAndRatingAsync(ReviewAndRating reviewAndRating);
        int RemoveReviewAndRating(ReviewAndRating reviewAndRating);
        Task<int> RemoveReviewAndRatingAsync(ReviewAndRating reviewAndRating);
        ReviewAndRating FindReviewAndRating(int Id);
        Task<ReviewAndRating> FindReviewAndRatingAsync(int Id);
    }
}

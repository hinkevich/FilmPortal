using FilmsPortalWebAPI.Models;

namespace FilmPortal.Models.Repo
{
    /// <summary>
    /// Interfece realizes repository pattern for FilmController
    /// </summary>
    public interface IReviewRepositiry
    {
        /// <summary>
        /// Demands to return all reviews as IQueryble.
        /// </summary>
        IQueryable<Review> Reviews { get; }

        /// <summary>
        ///  Demands to return all reviews as IEnumerable.
        /// </summary>
        /// <param name="filmId"></param>
        /// <returns></returns>
        public Task<IEnumerable<Review>> GetAllReviews(int filmId);

        /// <summary>
        /// Returns review by Id from Db.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<Review> GetReviewById(int id);

        /// <summary>
        /// Add review to Db.
        /// </summary>
        /// <param name="reviewDto"></param>
        /// <returns></returns>
        public Task<Review> AddReview(ReviewDto reviewDto);

        /// <summary>
        /// Delete review from db.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<bool> DeleteReview(int id);

        /// <summary>
        /// Changes review in Db.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="reviewDto"></param>
        /// <returns></returns>
        public Task<bool> UpdateReview(int id, ReviewDto reviewDto);
    }
}

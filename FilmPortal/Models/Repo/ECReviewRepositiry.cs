using FilmsPortal.Models;
using FilmsPortalWebAPI.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.EntityFrameworkCore;

namespace FilmPortal.Models.Repo
{
    /// <summary>
    /// Class for working with DbContext for ReviewsController. Pattern Repository.
    /// </summary>
    public class ECReviewRepositiry : IReviewRepositiry
    {
        private readonly FilmsPortalDbContext _context;

        /// <summary>
        /// Constructor. Gets DbContext.
        /// </summary>
        /// <param name="context"></param>
        public ECReviewRepositiry(FilmsPortalDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Returns list of reviews as Iqueryble.
        /// </summary>
        public IQueryable<Review> Reviews => _context.Reviews;

        /// <summary>
        /// Add review to Db.
        /// </summary>
        /// <param name="reviewDto"></param>
        /// <returns></returns>
        /// <exception cref="TaskCanceledException"></exception>
        public async Task<Review> AddReview(ReviewDto reviewDto)
        {
            if (_context.Reviews == null)
            {
               throw new TaskCanceledException();
            }
            var film = await _context.Films.FirstOrDefaultAsync(f=>f.Id==reviewDto.FilmId);

            if (film == null)
                return null!;

            var review = new Review {
                Title= reviewDto.Title,
                Description= reviewDto.Description,
                Stars = reviewDto.Stars,
                FilmId =film.Id,
                Film = film
            };

             _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            return review;

        }

        /// <summary>
        /// Delete review from Db.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteReview(int id)
        {
            if (_context.Reviews == null)
            {
                return false;
            }

            var review = await _context.Reviews.FindAsync(id);

            if (review == null)
            {
                return false;
            }

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Returns all review for film from Db.
        /// </summary>
        /// <param name="filmId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Review>> GetAllReviews(int filmId)
        {
            if (_context.Reviews == null)
            {
                return null!;
            }
            return await _context.Reviews.Where(r=>r.FilmId ==filmId).ToListAsync();
        }

        /// <summary>
        /// Returns review by Id from Db.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Review> GetReviewById(int id)
        {
            if (_context.Reviews == null)
            {
                return null!;
            }
            var review = await _context.Reviews.Where(r=>r.ReviewId==id).Include(r=>r.Film).FirstOrDefaultAsync();

            if (review == null)
            {
                return null!;
            }

            return review;
        }

        /// <summary>
        /// Changes review in Db.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="reviewDto"></param>
        /// <returns></returns>
        public async Task<bool> UpdateReview(int id, ReviewDto reviewDto)
        {
            var review = await _context.Reviews.FindAsync(id);

            if (review == null)
                return false; 

            _context.Entry(review).State = EntityState.Modified;

            review.Stars = reviewDto.Stars;
            review.Description = reviewDto.Description;
            review.Title = reviewDto.Title;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReviewExists(id))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }

            return true;

        }
        private bool ReviewExists(int id)
        {
            return (_context.Reviews?.Any(e => e.ReviewId == id)).GetValueOrDefault();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FilmsPortal.Models;
using FilmsPortalWebAPI.Models;
using FilmPortal.Models;
using FilmPortal.Models.Repo;

namespace FilmPortal.Controllers
{
    /// <summary>
    /// This controller works with film's review. He includes CRUD methods.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewRepositiry _repository;
        private readonly ILogger<ReviewsController> _logger;

        /// <summary>
        /// Constructor for ReviewsController. 
        /// </summary>
        /// <param name="repository">IreviewRepository</param>
        /// <param name="logger">ILogger</param>
        public ReviewsController(IReviewRepositiry repository, ILogger<ReviewsController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        /// <summary>
        /// Returns all reviews by film Id.
        /// </summary>
        /// <param name="filmId"> - Film Id, it is query parameter.</param>
        /// <returns></returns>
        // GET: api/Reviews
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Review>>> GetReviews(int filmId)
        {
            _logger.LogInformation($"Call {nameof(GetReviews)} - Time: {DateTime.UtcNow.ToLongTimeString()}");

            var reviews = await _repository.GetAllReviews(filmId);

            if (reviews == null)
            {
                return NotFound();
            }
            return reviews.ToList();
        }

        /// <summary>
        /// Returns one review by Id.
        /// </summary>
        /// <param name="id"> - part of route.</param>
        /// <returns></returns>
        // GET: api/Reviews/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Review>> GetReview(int id)
        {
            _logger.LogInformation($"Call {nameof(GetReview)} - Time: {DateTime.UtcNow.ToLongTimeString()}");

            var review = await _repository.GetReviewById(id);

            if (review == null)
            {
                return NotFound();
            }

            return review;
        }

        /// <summary>
        /// Changes review by Id.
        /// </summary>
        /// <param name="id"> - part of route.</param>
        /// <param name="reviewDto"> - query parameter. Dto jbject.</param>
        /// <returns></returns>
        // PUT: api/Reviews/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReview(int id, ReviewDto reviewDto)
        {
            _logger.LogInformation($"Call {nameof(PutReview)} - Time: {DateTime.UtcNow.ToLongTimeString()}");

            return await _repository.UpdateReview(id, reviewDto) ? NoContent() : NotFound();

        }

        /// <summary>
        /// Add review.
        /// </summary>
        /// <param name="reviewDto"> - Dto object.</param>
        /// <returns></returns>
        // POST: api/Reviews
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Review>> PostReview(ReviewDto reviewDto)
        {

            _logger.LogInformation($"Call {nameof(PostReview)} - Time: {DateTime.UtcNow.ToLongTimeString()}");

            try
            {
                var review = await _repository.AddReview(reviewDto);
                if (review != null)
                {
                    return CreatedAtAction(nameof(GetReview), new { id = review.ReviewId }, review);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (TaskCanceledException)
            {
                _logger.LogWarning($"Call {nameof(PostReview)}, return from repository: {typeof(TaskCanceledException)}: - Time: {DateTime.UtcNow.ToLongTimeString()}");

                return Problem("Entity set 'FilmsPortalDbContext.Reviews'  is null.");
            }
        }

        /// <summary>
        /// Delete review by Id.
        /// </summary>
        /// <param name="id"> - part of route.</param>
        /// <returns></returns>
        // DELETE: api/Reviews/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReview(int id)
        {
            _logger.LogInformation($"Call {nameof(PostReview)} - Time: {DateTime.UtcNow.ToLongTimeString()}");

            return await _repository.DeleteReview(id) ? NoContent() : NotFound();
            
        }
       
    }
}

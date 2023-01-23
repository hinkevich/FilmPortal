using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FilmsPortal.Models;
using FilmsPortalWebAPI.Models;
using FilmPortal.Models.Repo;
using FilmPortal.Models;

namespace FilmPortal.Controllers
{
    /// <summary>
    /// Implements CRUD shema for Film
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class FilmsController : ControllerBase
    {
        private readonly IFilmsRepository _repositiry;
        private readonly ILogger<FilmsController> _logger;

        /// <summary>
        /// Constructor for FilmsController
        /// </summary>
        /// <param name="repository">IFilmRepository</param>
        /// <param name="logger">ILogger</param>
        public FilmsController(IFilmsRepository repository, ILogger<FilmsController> logger)
        {
            _logger = logger;
            _repositiry = repository;

        }

        /// <summary>
        /// Returns All films
        /// </summary>
        /// <remarks><b>Return: </b>Positive result: Returns All films; Negative result: Not Found;
        /// </remarks>
        /// <returns></returns>
        // GET: api/Films
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Film>>> GetFilms()
        {
            _logger.LogInformation($"Call {nameof(GetFilms)} - Time: {DateTime.UtcNow.ToLongTimeString()}");

            var films = await _repositiry.GetAllFilms();

            if (films == null)
            {
                return NotFound();
            }

            return films.ToList();
        }

        /// <summary>
        /// Returns film by Id
        /// </summary>
        /// <param name="id">Route parameter</param>
        /// <remarks><b>Return: </b>Positive result: Returns films; Negative result: Not Found;
        /// </remarks>
        // GET: api/Films/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Film>> GetFilm(int id)
        {
            _logger.LogInformation($"Call {nameof(GetFilm)} - Time: {DateTime.UtcNow.ToLongTimeString()}");

            var film = await _repositiry.GetFilmById(id);

            if (film == null)
            {
                return NotFound();
            }

            return film;
        }

        /// <summary>
        /// Update film by Id
        /// </summary>
        /// <param name="id"> film Id</param>
        /// <param name="filmDto"> - DTO object for to change film.</param>
        /// <remarks><b>Return: </b>Positive result: No Content; Negative result: Not Found or Bad request;
        /// </remarks>
        // PUT: api/Films/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFilm(int id, FilmCreateDto filmDto)
        {
            _logger.LogInformation($"Call {nameof(PutFilm)} - Time: {DateTime.UtcNow.ToLongTimeString()}");

            return await _repositiry.UpdateFilm(id, filmDto) ? NoContent() : NotFound();

        }

        /// <summary>
        /// Add new film.
        /// </summary>
        /// <param name="filmDto">DTO object for film.</param>
        /// <remarks><b>Return: </b>Positive result: CreatedAction "GetFilm"; Negative result: Problem;
        /// </remarks>
        // POST: api/Films
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Film>> PostFilm(FilmCreateDto filmDto)
        {
            _logger.LogInformation($"Call {nameof(PostFilm)} - Time: {DateTime.UtcNow.ToLongTimeString()}");

            try
            {
                var film = await _repositiry.AddFilm(filmDto);

                return CreatedAtAction("GetFilm", new { id = film.Id }, film);
            }
            catch (TaskCanceledException)
            {
                _logger.LogWarning($"Call {nameof(PostFilm)}, return from repository: {typeof(TaskCanceledException)}: - Time: {DateTime.UtcNow.ToLongTimeString()}");

                return Problem("Entity set 'FilmsPortalDbContext.Films'  is null.","",503);
            }

        }

        /// <summary>
        /// Delete film by Id
        /// </summary>
        /// <param name="id"></param>
        /// <remarks><b>Return: </b>Positive result: No Content; Negative result: Not Found;
        /// </remarks>
        // DELETE: api/Films/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFilm(int id)
        {
            _logger.LogInformation($"Call {nameof(DeleteFilm)} - Time: {DateTime.UtcNow.ToLongTimeString()}");

            return await _repositiry.DeleteFilm(id) ? NoContent() : NotFound();

        }

    }
}

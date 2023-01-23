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
using System.Net;

namespace FilmPortal.Controllers
{
    /// <summary>
    /// CRUD Methods for Actors.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ActorsController : ControllerBase
    {
        private readonly IActorsRepository _repository;
        private readonly ILogger<ActorsController> _logger;

        /// <summary>
        /// Constructor. 
        /// </summary>
        public ActorsController(IActorsRepository repository, ILogger<ActorsController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        /// <summary>
        /// Returns list all actors, without list of films.
        /// </summary>
        /// <remarks> <b>Return: </b>Positive result: Returns all actors; Negative result: Not Found;
        /// </remarks>
        /// <returns></returns>
        // GET: api/Actors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Actor>>> GetActors()
        {
            _logger.LogInformation($"Call {nameof(GetActors)} - Time: {DateTime.UtcNow.ToLongTimeString()}");

            var actors = await _repository.GetAllActors();

            if (actors == null || actors.Count == 0)
            {
                return NotFound();
            }

            return actors;

        }

        /// <summary>
        /// Returns Actor by. Includes list of films.
        /// </summary>
        /// <remarks><b>Return: </b>Positive result: Returns actor; Negative result: Not Found;
        /// </remarks>
        /// <returns></returns>
        // GET: api/Actors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Actor>> GetActor(int id)
        {
            _logger.LogInformation($"Call {nameof(GetActor)} - Time: {DateTime.UtcNow.ToLongTimeString()}");

            if (id <= 0)
            {
                return BadRequest();
            }
            var actor = await _repository.GetActor(id);

            if (actor == null)
            {
                return NotFound();
            }

            return actor;
        }

        /// <summary>
        /// Changes actor by Id.
        /// </summary>
        /// <remarks><b>Return: </b>Positive result: No content; Negative result: Not Found or Bad Request;
        /// </remarks>
        // PUT: api/Actors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutActor(int id, ActorCreateDto actor)
        {
            _logger.LogInformation($"Call {nameof(PutActor)} - Time: {DateTime.UtcNow.ToLongTimeString()}");

            if (actor == null || id <= 0)
            {
                return BadRequest();
            }

            var resultUpdateActor = await _repository.UpdateActor(id, actor);
            if (resultUpdateActor)
            {
                return NoContent();
            }
            else
            {
                return NotFound();
            }

        }

        /// <summary>
        /// Add new actor without bindiing with film.
        /// </summary>
        /// <remarks><b>Return: </b>Positive result: Returns actor; Negative result: Problem;
        /// </remarks>
        // POST: api/Actors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Actor>> CreateFreeActor(ActorCreateDto actorCreateDto)
        {
            _logger.LogInformation($"Call {nameof(CreateFreeActor)} - Time: {DateTime.UtcNow.ToLongTimeString()}");

            var actor = await _repository.CreateFreeActor(actorCreateDto);
            if (actor == null)
            {
                _logger.LogWarning($"Call {nameof(CreateFreeActor)}, WARNING!: Cannot create Actor: - Time: {DateTime.UtcNow.ToLongTimeString()}");
                return Problem("Entity set 'FilmsPortalDbContext.Actors'  is null.", "DbSet<Actors>", 503);
            }
            return CreatedAtAction("GetActor", new { id = actor.Id }, actor);
        }


        /// <summary>
        /// Binds actor with film by Id.
        /// </summary>
        /// <remarks><b>Return: </b>Positive result: Returns actor; Negative result: Not Found;
        /// </remarks>
        // POST: api/Actors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("bind")]
        public async Task<ActionResult<Actor>> AddActorToFilm(int filmId, int actorId)
        {
            _logger.LogInformation($"Call {nameof(AddActorToFilm)} - Time: {DateTime.UtcNow.ToLongTimeString()}");

            try
            {
                var actor = await _repository.BindActorWithFilm(filmId, actorId);
                if (actor != null)
                {
                    return CreatedAtAction("GetActor", new { id = actor.Id }, actor);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (TaskCanceledException)
            {
                _logger.LogWarning($"Call {nameof(AddActorToFilm)}, WARNING!: Exeption {typeof(TaskCanceledException)}: {DateTime.UtcNow.ToLongTimeString()}");

                return Problem("Entity set 'FilmsPortalDbContext.Actors' or 'FilmsPortalDbContext.Films' is null.", "", 503);
            }

        }

        /// <summary>
        /// Add new actor, who is binded with having film.
        /// </summary>
        /// <remarks><b>Return: </b>Positive result: Returns actor; Negative result: Not Found or Problem;
        /// </remarks>
        [HttpPost("film")]
        public async Task<ActionResult<Actor>> CreateActorAndBindWithFilm(int filmId, ActorCreateDto actorCreateDto)
        {
            _logger.LogInformation($"Call {nameof(CreateActorAndBindWithFilm)} - Time: {DateTime.UtcNow.ToLongTimeString()}");

            try
            {
                var actor = await _repository.CreateAndBindActorWithFilm(filmId, actorCreateDto);
                if (actor != null)
                {
                    return CreatedAtAction("GetActor", new { id = actor.Id }, actor);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (TaskCanceledException)
            {
                _logger.LogWarning($"Call {nameof(CreateActorAndBindWithFilm)}, WARNING!: Exeption {typeof(TaskCanceledException)}: {DateTime.UtcNow.ToLongTimeString()}");
                return Problem("Entity set 'FilmsPortalDbContext.Films' is null.", "", 503);
            }

        }

        /// <summary>
        /// Delete actor by Id.
        /// </summary>
        /// <remarks><b>Return: </b>Positive result: Returns No content; Negative result: Not Found;
        /// </remarks>
        // DELETE: api/Actors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActor(int id)
        {
            _logger.LogInformation($"Call {nameof(DeleteActor)} - Time: {DateTime.UtcNow.ToLongTimeString()}");

            if (id <= 0) 
            {
                _logger.LogWarning($"Call {nameof(DeleteActor)}, WARNING!: Incorrect {nameof(id)}: {DateTime.UtcNow.ToLongTimeString()}");

                return BadRequest();
            }

            var result = await _repository.DeleteActor(id);

            if (result)
            {
                return NoContent();
            }
            else
            {
                return NotFound();
            }
        }
    }
}

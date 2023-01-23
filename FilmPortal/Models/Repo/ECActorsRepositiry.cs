using FilmsPortal.Models;
using FilmsPortalWebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FilmPortal.Models.Repo
{
    /// <summary>
    /// Class for working with DbContext for ActorsController. Pattern Repository.
    /// </summary>
    public class ECActorsRepositiry : IActorsRepository
    {
        private readonly FilmsPortalDbContext _context;

        /// <summary>
        /// Constructor. Gets DbContext.
        /// </summary>
        /// <param name="context"></param>
        public ECActorsRepositiry(FilmsPortalDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Returns list of actors as Iqueryble.
        /// </summary>
        public IQueryable<Actor> Actors => _context.Actors;

        /// <summary>
        /// Bind Actor with Film.
        /// </summary>
        /// <param name="filmId"></param>
        /// <param name="actorId"></param>
        /// <returns></returns>
        /// <exception cref="TaskCanceledException"></exception>

        public async Task<Actor> BindActorWithFilm(int filmId, int actorId)
        {
            if (_context.Actors == null || _context.Films == null)
            {
                throw new TaskCanceledException();
            }

            var actor = await _context.Actors.FindAsync(actorId);

            if (actor == null)
                return null!;

            var film = await _context.Films.Where(f => f.Id == filmId).Include(f => f.Actors).FirstOrDefaultAsync();

            if (film == null)
                return null!;

            film.Actors.Add(actor);
            await _context.SaveChangesAsync();
            return actor;

        }

        /// <summary>
        /// Create new actor and add relationship with film.
        /// </summary>
        /// <param name="filmId"></param>
        /// <param name="actorDto"></param>
        /// <returns></returns>
        /// <exception cref="TaskCanceledException"></exception>
        public async Task<Actor> CreateAndBindActorWithFilm(int filmId, ActorCreateDto actorDto)
        {
            if (_context.Films == null)
                throw new TaskCanceledException();


            var film = await _context.Films.Where(f => f.Id == filmId).Include(f => f.Actors).FirstOrDefaultAsync();

            if (film == null)
                return null!;

            Actor actor = new Actor
            {
                FirstName = actorDto.FirstName,
                LastName = actorDto.LastName,
            };

            film.Actors.Add(actor);
            await _context.SaveChangesAsync();
            return actor;
        }

        /// <summary>
        /// Add new actor to Db.
        /// </summary>
        /// <param name="actorDto"></param>
        /// <returns></returns>
        public async Task<Actor> CreateFreeActor(ActorCreateDto actorDto)
        {
            if (_context.Actors == null)
            {
                return null!;
            }

            Actor actor = new Actor
            {
                FirstName = actorDto.FirstName,
                LastName = actorDto.LastName,

            };
            _context.Actors.Add(actor);
            await _context.SaveChangesAsync();
            return actor;
        }

        /// <summary>
        /// Delete Actor from Db.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteActor(int id)
        {
            if (_context.Actors == null)
            {
                return false;
            }
            var actor = await _context.Actors.FindAsync(id);
            if (actor == null)
            {
                return false;
            }

            _context.Actors.Remove(actor);
            await _context.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Returns Actor by. Include list of films.
        /// </summary>
        public async Task<Actor?> GetActor(int id)
        {
            return await _context.Actors.Where(a => a.Id == id).Include(a => a.Films).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Returns list of actors.
        /// </summary>
        public async Task<List<Actor>> GetAllActors()
        {
            return await _context.Actors.ToListAsync();
        }

        /// <summary>
        /// Changes actor.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="actorDto"></param>
        /// <returns></returns>
        public async Task<bool> UpdateActor(int id, ActorCreateDto actorDto)
        {
            var actor =await _context.Actors.FindAsync(id);
            if (actor == null)
                return false;
            actor.FirstName = actorDto.FirstName; 
            actor.LastName = actorDto.LastName;
            _context.Entry(actor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ActorExists(id))
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

        private bool ActorExists(int id)
        {
            return (_context.Actors?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

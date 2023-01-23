using FilmsPortal.Models;
using FilmsPortalWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;

namespace FilmPortal.Models.Repo
{
    /// <summary>
    /// Class for working with Dbcontext for FilmsController. Pattern Repository.
    /// </summary>
    public class ECFilmRepository : IFilmsRepository
    {
        private readonly FilmsPortalDbContext _context;

        /// <summary>
        /// Returns list of films as Iqueryble.
        /// </summary>
        public IQueryable<Film> Films => _context.Films;

        /// <summary>
        /// Constructor. Gets DbContext.
        /// </summary>
        /// <param name="context"></param>
        public ECFilmRepository(FilmsPortalDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Add film to Db.
        /// </summary>
        /// <param name="filmDto"></param>
        /// <returns></returns>
        /// <exception cref="TaskCanceledException"></exception>
        public async Task<Film> AddFilm(FilmCreateDto filmDto)
        {
            if (_context.Films == null)
                throw new TaskCanceledException();

            if (filmDto == null)  
                return null!;
            var film = new Film
            {
                Title = filmDto.Title,
            };

            _context.Films.Add(film);

            await _context.SaveChangesAsync();
            return film;
        }

        /// <summary>
        /// Delete film from Db.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteFilm(int id)
        {
            if (_context.Films == null)
            {
                return false;
            }
            var film = await _context.Films.FindAsync(id);
            if (film == null)
            {
                return false;
            }

            _context.Films.Remove(film);
            await _context.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Returns all films from Db.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Film>> GetAllFilms()
        {
            if (_context.Films == null)
            {
                return null!;
            }
            return await _context.Films.ToListAsync();
        }

        /// <summary>
        /// Returns film from Db.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Film> GetFilmById(int id)
        {
            if (_context.Films == null)
            {
                return null!;
            }
            var film = await _context.Films.Where(f=>f.Id ==id).Include(f=>f.Actors).FirstOrDefaultAsync();

            if (film == null)
            {
                return null!;
            }

            return film;
        }

        /// <summary>
        /// Change film in Db.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="filmDto"></param>
        /// <returns></returns>
        public async Task<bool> UpdateFilm(int id, FilmCreateDto filmDto)
        {

            var film = await _context.Films.FindAsync(id);

            if (film == null)
                return false;

           film.Title = filmDto.Title;

            _context.Entry(film).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FilmExists(id))
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

        private bool FilmExists(int id)
        {
            return (_context.Films?.Any(e => e.Id == id)).GetValueOrDefault();
        }


    }
}

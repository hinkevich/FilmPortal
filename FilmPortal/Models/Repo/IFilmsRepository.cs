using FilmsPortalWebAPI.Models;

namespace FilmPortal.Models.Repo
{
    /// <summary>
    /// Interfece realizes repository pattern for FilmController
    /// </summary>
    public interface IFilmsRepository
    {
        /// <summary>
        /// Demands to return all film as IQueryble.
        /// </summary>
        IQueryable<Film> Films { get; }


        /// <summary>
        /// Demands to return all film as IEnumerable.
        /// </summary>
        public Task<IEnumerable<Film>> GetAllFilms();

        /// <summary>
        /// Demands to return film by Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<Film> GetFilmById(int id);

        /// <summary>
        /// Demand to add film.
        /// </summary>
        /// <param name="film"></param>
        /// <returns></returns>
        public Task<Film> AddFilm(FilmCreateDto film);

        /// <summary>
        /// demands to delete film by Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<bool> DeleteFilm(int id);

        /// <summary>
        /// Demands to change film.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="film"></param>
        /// <returns></returns>
        public Task<bool> UpdateFilm(int id, FilmCreateDto film);
    }
}

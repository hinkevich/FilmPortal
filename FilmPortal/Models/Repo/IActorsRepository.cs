using FilmsPortalWebAPI.Models;

namespace FilmPortal.Models.Repo
{
    /// <summary>
    /// Interfece realizes repository pattern for ActorsController
    /// </summary>
    public interface IActorsRepository
    {
        /// <summary>
        /// Demand to return all Actors.
        /// </summary>
        IQueryable<Actor> Actors { get; }

        /// <summary>
        /// Demand to return all actors for async method.
        /// </summary>
        public Task<List<Actor>> GetAllActors();

        /// <summary>
        /// Demand returnone actor for async method.
        /// </summary>
        public Task<Actor?> GetActor(int id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="actorDto"></param>
        /// <returns></returns>
        public Task<bool> UpdateActor(int id, ActorCreateDto actorDto);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actorDto"></param>
        /// <returns></returns>
        public Task<Actor> CreateFreeActor(ActorCreateDto actorDto);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filmId"></param>
        /// <param name="actorId"></param>
        /// <returns></returns>
        public Task<Actor> BindActorWithFilm(int filmId, int actorId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filmId"></param>
        /// <param name="actorDto"></param>
        /// <returns></returns>
        public Task<Actor> CreateAndBindActorWithFilm(int filmId, ActorCreateDto actorDto);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<bool> DeleteActor(int id);

    }
}

using FilmPortal.Models;
using FilmPortal.Models.Repo;
using FilmsPortalWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilmPortal.Tests.FakeRepositories
{
    internal class FakeActorsRepository : IActorsRepository
    {
        public static bool SwitchResult { get; set; }

        public IQueryable<Actor> Actors => GetActorsForTests().AsQueryable();

        public  Task<Actor> BindActorWithFilm(int filmId, int actorId)
        {
            if (!SwitchResult)
                throw new TaskCanceledException();
           
            var actor = GetActorsForTests().FirstOrDefault(a => a.Id == actorId);

            if (actor == null)
                return Task.FromResult<Actor>(null!);

            var film = GetFilmsForTests().FirstOrDefault(f => f.Id == filmId);

            if (film == null)
                return Task.FromResult<Actor>(null!);

            return Task.FromResult(actor);
        }

        public  Task<Actor> CreateAndBindActorWithFilm(int filmId, ActorCreateDto actorDto)
        {
            if (!SwitchResult)
                throw new TaskCanceledException();

            var film = GetFilmsForTests().FirstOrDefault(f => f.Id == filmId);

            if (film == null)
                return Task.FromResult<Actor>(null!);

            return Task.FromResult( new Actor()
            {
                Id = 1,
                FirstName = actorDto.FirstName,
                LastName = actorDto.LastName,
            });
        }

        public Task<Actor> CreateFreeActor(ActorCreateDto actorDto)
        {
            if (SwitchResult)
            {
                return Task.FromResult( new Actor()
                {
                    Id = 1,
                    LastName = actorDto.LastName,
                    FirstName = actorDto.FirstName,

                });
            }
            else
            {
                return Task.FromResult<Actor>(null!);
            }
        }

        public Task<bool> DeleteActor(int id)
        {
            var actor = GetActorsForTests().FirstOrDefault(a => a.Id == id);

            if (actor == null)
                return Task.FromResult(false);

            return Task.FromResult(true);
        }

        public Task<Actor?> GetActor(int id)
        {
            var actors = GetActorsForTests();
            return Task.FromResult(actors.FirstOrDefault(a => a.Id == id));
        }

        public Task<List<Actor>> GetAllActors()
        {
            return Task.FromResult(GetActorsForTests());
        }

        public Task<bool> UpdateActor(int id, ActorCreateDto actor)
        {
            var actors = GetActorsForTests();
            if (actors.FirstOrDefault(a => a.Id == id) != null)
                return Task.FromResult(true);
            return Task.FromResult(false);
        }

        // Testing Data
        private static  List<Actor> GetActorsForTests()
        {
            var actors = new List<Actor>();
            actors.Add(new Actor() { Id = 1, FirstName = "Brad", LastName = "Pitt", });
            actors.Add(new Actor() { Id = 2, FirstName = "Edward", LastName = "Norton", });
            actors.Add(new Actor() { Id = 3, FirstName = "Nicole", LastName = "Kidman", });
            return actors;
        }

        private static List<Film> GetFilmsForTests()
        {
            var films = new List<Film>();

            films.Add(new Film
            {
                Id = 1,
                Title = "Fight Club",
                Actors = new[] { new Actor
                {
                    Id = 1,
                    FirstName = "Brad",
                    LastName = "Pitt"

                },
                new Actor
                {
                    Id = 2,
                    FirstName = "Edward",
                    LastName = "Norton"

                }
                },
                Reviews = new[]
                {
                    new Review
                    {
                        ReviewId = 1,
                        Title = "Ok",
                        Description = "Its very interesting film",
                        Stars = FilmStars.fiveStars,
                        FilmId = 1,
                    },

                    new Review
                    {
                        ReviewId = 2,
                        Title = "Not Bad",
                        Description = "Can watch one time",
                        Stars = FilmStars.fourStars,
                        FilmId = 1,
                    },
                }
            });
            films.Add(new Film
            {
                Id = 2,
                Title = "Terminator",
                Actors = new[] { new Actor
                {
                    Id = 3,
                    FirstName = "Arnold",
                    LastName = "Shwarzenneger"

                },
                new Actor
                {
                    Id = 4,
                    FirstName = "Linda",
                    LastName = "Hamilton"

                }
                },
                Reviews = new[]
                {
                    new Review
                    {
                        ReviewId = 3,
                        Title = "Norm",
                        Description = "Second part is better.",
                        Stars = FilmStars.threeStars,
                        FilmId = 2,
                    },

                    new Review
                    {
                        ReviewId = 4,
                        Title = "it's Classicall",
                        Description = "I won't remember this film",
                        Stars = FilmStars.fiveStars,
                        FilmId = 2,
                    },
                }
            });

            return films;
        }
    }
}

using FilmsPortalWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilmPortal.Tests.TestData
{
    public static class FilmPortalTestData
    {
       public static List<Actor> GetActorsForTests()
        {
            var actors = new List<Actor>();
            actors.Add(new Actor() { Id = 1, FirstName = "Brad", LastName = "Pitt", });
            actors.Add(new Actor() { Id = 2, FirstName = "Edward", LastName = "Norton", });
            actors.Add(new Actor() { Id = 3, FirstName = "Nicole", LastName = "Kidman", });
            return actors;
        }

        public static List<Film> GetFilmsForTests()
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

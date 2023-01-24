using FilmsPortal.Models;
using FilmsPortalWebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FilmPortal.Models.Data
{
    /// <summary>
    /// Add some data to database for testing
    /// </summary>
    public static class SeedData
    {
        /// <summary>
        /// 
        /// </summary>
        public static object ComletionDate { get; private set; } = null!;

        /// <summary>
        /// Check data base. If db is emty, add some data to db.
        /// </summary>
        /// <param name="app"></param>
        public static void EnsurePopolate(WebApplication app)
        {
            FilmsPortalDbContext context = app.Services.CreateScope().ServiceProvider.GetRequiredService<FilmsPortalDbContext>();

            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }

            if (!context.Films.Any()||!context.Actors.Any())
            {

                var actorOne = new Actor
                {
                    FirstName = "Leonardo",
                    LastName = "Dicaprio",
                };
                var actorTwo = new Actor
                {
                    FirstName = "Leonardo",
                    LastName = "Dicaprio",
                };
                var filmOne = new Film
                {
                    Title = "Titanic"
                };

                var actorThree = new Actor
                {
                    FirstName = "Arnold",
                    LastName = "Schwarzenegger",
                };
                var actorFour = new Actor
                {
                    FirstName = "Linda",
                    LastName = "Hamilton",
                };
                var filmTwo = new Film
                {
                    Title = "Terminator"
                };

                context.Films.Add(filmOne);
                context.Films.Add(filmTwo);
                context.SaveChanges();
                context.Actors.Add(actorOne);
                context.Actors.Add(actorTwo);
                context.Actors.Add(actorThree);
                context.Actors.Add(actorFour);
                context.SaveChanges();
                
                actorOne = context.Actors.FirstOrDefault(a=>a.Id==1);
                actorTwo = context.Actors.FirstOrDefault(a => a.Id == 2);
                actorThree = context.Actors.FirstOrDefault(a => a.Id==3);
                actorFour = context.Actors.FirstOrDefault(a => a.Id==4);

                filmOne = context.Films.FirstOrDefault(a => a.Id == 1);
                filmTwo = context.Films.FirstOrDefault(a => a.Id == 2);

                filmOne!.Actors.Add(actorOne!);
                filmOne.Actors.Add(actorTwo!);
                filmTwo!.Actors.Add(actorThree!);
                filmTwo.Actors.Add(actorFour!);
                
                context.SaveChanges();
            }
        }
    }
}

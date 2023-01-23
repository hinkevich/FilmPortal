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
            //if (!context.Films.Any())
            //{
            //    context.Films.AddRange(
            //        new Film
            //        {
            //            Title = "Titanic",
            //            Actors = new[]
            //           {
            //               new Actor
            //               {
            //                   FirstName = "Leonardo",
            //                   LastName = "Dicaprio",
            //               },
            //                new Actor
            //               {
            //                   FirstName = "Kate",
            //                   LastName = "Winslet",
            //               }
            //           },
            //            Reviews = new[] 
            //            {
            //                new Review
            //                {
            //                    Title = "Titanic",
            //                    Description = "It's norm film",
            //                    Stars = FilmStars.fourStars
            //                },
            //                new Review
            //                {
            //                    Title = "Titanic",
            //                    Description = "Not bad",
            //                    Stars = FilmStars.threeStars,
            //                },
            //                new Review
            //                {
            //                    Title = "Titanic",
            //                    Description = "Very good",
            //                    Stars = FilmStars.fiveStars,
            //                },

            //            }
            //        },
            //        new Film
            //        {
            //            Title = "Terminator",
            //            Actors = new[]
            //           {
            //               new Actor
            //               {
            //                   FirstName = "Arnold",
            //                   LastName = "Schwarzenegger",
            //               },
            //                new Actor
            //               {
            //                   FirstName = "Linda",
            //                   LastName = "Hamilton",
            //               },
                           
            //           },
            //            Reviews = new[]
            //            {
            //                new Review
            //                {
            //                    Description = "It's classic",
            //                    Title = "Terminator",
            //                    Stars = FilmStars.fourStars
            //                },
            //                new Review
            //                {
            //                    Title = "Terminator",
            //                    Description = "I was shocked",
            //                    Stars = FilmStars.fiveStars,
            //                },
            //                new Review
            //                {
            //                    Title = "Terminator",
            //                    Description = "Arny is very young",
            //                    Stars = FilmStars.fiveStars,
            //                },
            //            }
            //        } 
            //        );
            //    context.SaveChanges();
            //}
        }
    }
}

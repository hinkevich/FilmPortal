using FilmsPortalWebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FilmsPortal.Models
{
    /// <summary>
    /// DbContext
    /// </summary>
    public class FilmsPortalDbContext:DbContext
    {
        /// <summary>
        /// Constructor for  FilmsPortalDbContext.
        /// </summary>
        /// <param name="options"></param>
        public FilmsPortalDbContext(DbContextOptions<FilmsPortalDbContext> options):base(options)
        {

        }
       
        /// <summary>
        /// DbSet for films.
        /// </summary>
        public DbSet<Film> Films { get; set; }

        /// <summary>
        /// DbSet for Reviews.
        /// </summary>
        public DbSet<Review> Reviews { get; set; }

        /// <summary>
        /// DbSet for Actors.
        /// </summary>
        public DbSet<Actor> Actors { get; set; }

    } 
}
 
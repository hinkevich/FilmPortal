using Microsoft.Build.Framework;
using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace FilmsPortalWebAPI.Models
{
    /// <summary>
    /// Model class Film
    /// </summary>
    public class Film
    {

        /// <summary>
        /// Film Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of film
        /// </summary>
        [Required(ErrorMessage = "Lenght can be more than 1, and less than 150")]
        [Display(Name = "First name")]
        [StringLength(150, MinimumLength = 1)]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Navigation property, relationship: one to many.
        /// </summary>
        public ICollection<Review> Reviews { get; set; } = null!;

        /// <summary>
        /// Navigation property, relationship: many to many.
        /// </summary>
        public ICollection<Actor> Actors { get; set; } = null!; 
    }
}

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace FilmsPortalWebAPI.Models
{
    /// <summary>
    /// Model Db for actor
    /// </summary>
    public class Actor
    {
        /// <summary>
        /// Id 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Firstname, Length from 1 to 150.
        /// </summary>
        [Required(ErrorMessage = "Lenght can be more than 1, and less than 150")]
        [Display(Name = "First name")]
        [StringLength(150, MinimumLength = 1)]
        public string FirstName { get; set; } = null!;

        /// <summary>
        /// LastName, Length from 1 to 150.
        /// </summary>
        [Required(ErrorMessage = "Lenght can be more than 3, and less than 150")]
        [Display(Name = "Last name")]
        [StringLength(150, MinimumLength = 1)]
        public string LastName { get; set; } = null!;

        /// <summary>
        /// Navigation property, relationship: many to many.
        /// </summary>
        public ICollection<Film> Films { get; set; } = null!;
    }
}

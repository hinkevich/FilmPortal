using System.ComponentModel.DataAnnotations;

namespace FilmPortal.Models
{
    /// <summary>
    /// DTO object for making actor.
    /// </summary>
    public class ActorCreateDto
    {
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

    }
}

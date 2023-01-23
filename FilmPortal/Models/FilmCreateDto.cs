using System.ComponentModel.DataAnnotations;

namespace FilmPortal.Models
{
    /// <summary>
    /// DTO object to add a movie. 
    /// </summary>
    public class FilmCreateDto
    {
        /// <summary>
        /// Movie title.
        /// </summary>
        [Required(ErrorMessage = "Lenght can be more than 1, and less than 150")]
        [Display(Name = "First name")]
        [StringLength(150, MinimumLength = 1)]
        public string Title { get; set; } = null!;
    }
}

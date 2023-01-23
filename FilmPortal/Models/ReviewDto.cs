using FilmsPortalWebAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace FilmPortal.Models
{
    /// <summary>
    /// DTO object for creating review.
    /// </summary>
    public class ReviewDto
    {
        /// <summary>
        /// Review's title, min lenght: 1, max lenght: 150;
        /// </summary>
        [Required(ErrorMessage = "Lenght can be more than 3, and less than 150")]
        [Display(Name = "First name")]
        [StringLength(150, MinimumLength = 1)]
        public string Title { get; set; } = null!;

        /// <summary>
        /// Description, min lenght 2, min lenght: 2, max lenght: 450;
        /// </summary>
        [Required(ErrorMessage = "Lenght can be more than 2, and less than 450")]
        [Display(Name = "First name")]
        [StringLength(150, MinimumLength = 1)]
        public string Description { get; set; } = null!;

        /// <summary>
        /// Foreign key 
        /// </summary>
        public int FilmId { get; set; }

        /// <summary>
        /// Quality assessment parameter. From 0 to 4.
        /// </summary>
        /// <remarks>
        /// 0 : One Star;
        /// 1 : Two Stars; 
        /// 2 : Three Stars; 
        /// 3 : Four Stars; 
        /// 4 : Five Stars; 
        /// </remarks>
        [Required(ErrorMessage = "Range 0-4")]
        [Display(Name = "First name")]
        [Range(0, 4)]
        public FilmStars Stars { get; set; }
    }
}

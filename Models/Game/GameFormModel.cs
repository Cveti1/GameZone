using System.ComponentModel.DataAnnotations;
using System.Configuration;
using GameZone.Models.Genre;
using Microsoft.CodeAnalysis;
using Microsoft.VisualBasic;
using static GameZone.Data.DataConstants;

namespace GameZone.Models.Game
{
    public class GameFormModel
    {
        [Required]
        [StringLength(TitleMaxName, MinimumLength = TitleMinName)]
        public string Title { get; set; } = null!;



     
        public string? ImageUrl { get; set; } = null!;


        [Required]
        [StringLength(DescriptionMaxName, MinimumLength = DescriptionMinName)]
        public string Description { get; set; } = null!;



        [Required] 
        public DateTime ReleasedOn { get; set; } 
        


        [Required]
        public int GenreId { get; set; }


        public virtual IEnumerable<GenreViewModel> Genres { get; set; } = new List<GenreViewModel>();
    }
}

using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.CodeAnalysis;
using static GameZone.Data.DataConstants;

namespace GameZone.Data.Models
{
    public class Game
    {
        [Key]
        public int Id { get; set; }


        [Required]
        [StringLength(TitleMaxName)]
        public string Title { get; set; } = null!;



        [Required]
        [StringLength(DescriptionMaxName)]
        public string Description { get; set; } = null!;


        public string? ImageUrl { get; set; }
       



        [Required]
        public string PublisherId { get; set; } = null!;



        [ForeignKey(nameof(PublisherId))]
        public IdentityUser Publisher { get; set; } = null!;


        public DateTime ReleasedOn { get; set; }

     

        [Required]
        public int GenreId { get; set; }



        [ForeignKey(nameof(GenreId))]
        public Genre Genre { get; set; } = null!;

        public ICollection<GamerGame> GamersGames { get; set; } = new List<GamerGame>();

    }
}

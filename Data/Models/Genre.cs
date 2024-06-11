using System.ComponentModel.DataAnnotations;
using static GameZone.Data.DataConstants;

namespace GameZone.Data.Models
{
    public class Genre
    {

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(GenreNameMax)]
        public string Name { get; set; } = null!;

        public virtual ICollection<Game> Games { get; set; } = new List<Game>();

        
    }
}

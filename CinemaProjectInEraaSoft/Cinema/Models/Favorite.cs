using Microsoft.EntityFrameworkCore;

namespace Cinema.Models
{
    [PrimaryKey(nameof(FilmId), nameof(ApplicationId))]

    public class Favorite
    {
        public string ApplicationId { get; set; } = null!;
        public ApplicationUser ApplicationUser { get; set; } = null!;

        public int FilmId { get; set; }
        public Film Film { get; set; } = null!;
    }
}

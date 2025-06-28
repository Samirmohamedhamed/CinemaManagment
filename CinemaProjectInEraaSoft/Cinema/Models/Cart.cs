using Microsoft.EntityFrameworkCore;

namespace Cinema.Models
{ 
[PrimaryKey(nameof(FilmId), nameof(ApplicationId))]
public class Cart
{

    public int FilmId { get; set; }
    public Film Film { get; set; } = null!;
    public string ApplicationId { get; set; } = null!;
    public ApplicationUser Application { get; set; } = null!;
    public int CountOFTicet { get; set; }
}
}
